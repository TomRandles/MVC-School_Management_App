using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.ViewModel;
using ServicesLib.Domain.Models;
using ServicesLib.Domain.Utilities;
using ServicesLib.Domain.ViewModel;

namespace SchoolManagement.Controllers
{
    public class AuthController : Controller
    {

        private readonly IConfiguration config;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AuthController(IConfiguration config,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.config = config;
            this.userManager = userManager;
            this.signInManager = signInManager;

        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVm login)
        {
            try
            {
                var user = await userManager.FindByNameAsync(login.UserName);

                if (user != null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);
                    if (result.Succeeded)
                    {
                        var appUser = userManager.Users.FirstOrDefault(x => x.NormalizedUserName == login.UserName.ToUpper());
                        string token =await GenerateTokenAsync(appUser);
                        if (token != null)
                        {
                            HttpContext.Session.SetString("JWToken", token);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ViewBag.ErrorMsg = "Incorrect UserId or Password";
                return View(login);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeAdminPassword()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ChangeAdminPassword(ChangePasswordViewModel model)
        {
            try
            {

                if (!model.NewPassword.Equals(model.ConfirmPassword))
                {
                    ViewBag.ErrorMsg = "New Passoword and Confirm Password Doesn't Match";
                    return View(model);
                }

                if (model.NewPassword.Equals(model.CurrentPassword))
                {
                    ViewBag.ErrorMsg = "Current Password and New Password are Same";
                    return View(model);
                }

                if (model.NewPassword.Length < 4)
                {
                    ViewBag.ErrorMsg = "Minimum 4 Digid Required";
                    return View(model);
                }

                var user = await userManager.FindByNameAsync("Admin");
                var result = await signInManager.CheckPasswordSignInAsync(user, model.CurrentPassword, false);

                if (result.Succeeded)
                {
                    var isChange = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (isChange.Succeeded)
                    {
                        return RedirectToAction(nameof(Logout));
                    }
                }

                ViewBag.ErrorMsg = "Incorrect Current Password";
                return View(model);


            }
            catch (Exception)
            {

                return Unauthorized();
            }


        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            //Creating Jwt Token
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Surname,user.FullName),
            };


            //Adding token in the token
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key  = Encoding.UTF8.GetBytes(config.GetSection("Application:Token").Value);
            //Generate Token for user 
            var JWToken = new JwtSecurityToken(
                issuer: config.GetSection("Application:DomainName").Value,
                audience: config.GetSection("Application:DomainName").Value,
                claims: claims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(7)).DateTime,
                //Using HS256 Algorithm to encrypt Token  
                signingCredentials: new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return token;

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}