using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagement.Utilities;
using ServicesLib.Domain.Models;
using ServicesLib.Domain.Utilities;
using ServicesLib.Services.Repositories;
using ServicesLib.Services.Repository.Generic;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IProgrammeRepository programmeRepository;
        private readonly UserManager<AppUser> userManager;

        public TeacherController(ITeacherRepository teacherRepository,
                                 IProgrammeRepository programmeRepository,
                                 UserManager<AppUser> userManager)
        {
            this.teacherRepository = teacherRepository;
            this.programmeRepository = programmeRepository;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        // GET: Admin/RegisterTeacher
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            try
            {
                ViewBag.ProgrammeList = ViewBag.ProgrammeList = await programmeRepository.AllAsync();
                ViewBag.TeacherId = GetTeacherId();
                return View();
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        // GET: Admin/RegisterTeacher
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Teacher teacher, IFormFile image)
        {
            try
            {
                if (image != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        image.CopyTo(stream);
                        teacher.TeacherImage = stream.ToArray();
                    }
                }

                if (ModelState.IsValid)
                {
                    await teacherRepository.AddAsync(teacher);
                    await teacherRepository.SaveChangesAsync();

                    string fullName = teacher.FirstName + " " + teacher.SurName;
                    await CreateTeacherAsUserAsync(teacher.TeacherID, teacher.Password, fullName);

                    ViewBag.SuccessMsg = "Teacher Successfully Registered";

                    ModelState.Clear();

                    return RedirectToAction ("Details", new { id = teacher.TeacherID});
                }
                return View(teacher);
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Teacher
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await teacherRepository.AllIncludeProgrammeAsync());
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpGet] // GET: Teacher/Details/5
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMsg = $"Invalid teacher ID: {id}";
                    return RedirectToAction("Unknown", "Error");
                }

                var teacher = await teacherRepository.FindByIdIncludeProgrammeAsync(id);
                if (teacher == null)
                {
                    ViewBag.ErrorMsg = $"Teacher not found: {id}";
                    return RedirectToAction("NotFoundPage", "Error");
                }

                return View(teacher);
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] // GET: Teacher/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await teacherRepository.FindByIdIncludeProgrammeAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            ViewData["ProgrammeID"] = new SelectList(await programmeRepository.AllAsync(),
                                                     "ProgrammeID",
                                                     "ProgrammeID",
                                                     teacher.ProgrammeID);
            return View(teacher);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
                                              [Bind("TeacherID,FirstName,SurName,Password,AddressOne,AddressTwo,Town,County,MobilePhoneNumber,EmailAddress,EmergencyMobilePhoneNumber,TeacherPPS,ProgrammeFeePaid,GenderType,FullOrPartTime,TeacherImage,ProgrammeID")] Teacher teacher)
        {
            if (id != teacher.TeacherID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    teacherRepository.Update(teacher);
                    await teacherRepository.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = teacher.TeacherID });
                }
                catch (DataAccessException e)
                {
                    ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                    if (!TeacherExists(teacher.TeacherID))
                    {
                        return NotFound();
                    }
                    return RedirectToAction("Unknown", "Error");
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                    return RedirectToAction("Unknown", "Error");
                }
            }
            return View(teacher);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] // GET: Teacher/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("NotFoundPage", "Error");
                }

                var teacher = await teacherRepository.FindByIdIncludeProgrammeAsync(id);
                if (teacher == null)
                {
                    return RedirectToAction("NotFoundPage", "Error");
                }
                return View(teacher);
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await teacherRepository.DeleteTeacherAsync(id);
                await teacherRepository.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }
        private bool TeacherExists(string id)
        {
            var teacher = teacherRepository.FindAsync(t => t.TeacherID == id);
            return (teacher != null) ? true : false;
        }
        private async Task CreateTeacherAsUserAsync(string teacherID, string password, string fullName)
        {
            var user = new AppUser();
            user.UserName = teacherID;
            user.FullName = fullName;
            var result = await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "Teacher");
        }
        private string GetTeacherId()
        {
            Random r = new Random();
            return "T" + r.Next(100_000, 999_999).ToString();
        }
    }
}