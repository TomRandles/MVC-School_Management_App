using Microsoft.AspNetCore.Mvc;

namespace SchoolManagement.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult Unauthorize()
        {
            return View();
        }

        public IActionResult Unknown()
        {
            return View();
        }

        public IActionResult Delete(string errorMsg)
        {
            ViewBag.ErrorMsg = errorMsg;
            return View();
        }
    }
}