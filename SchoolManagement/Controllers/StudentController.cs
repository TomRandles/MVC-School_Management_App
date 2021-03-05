using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using ServicesLib.Services.Repositories;
using SchoolManagement.ViewModel;
using ServicesLib.Domain.Utilities;
using ServicesLib.Services.Repository.Generic;
using SchoolManagement.Utilities;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IStudentRepository studentRepository;
        private readonly IRepository<Programme> programmeRepository;
        private readonly IAssessmentResultRepository assessmentResultRepository;

        public StudentController(UserManager<AppUser> userManager,
                                 IStudentRepository studentRepository,
                                 IProgrammeRepository programmeRepository,
                                 IAssessmentResultRepository assessmentResultRepository)
        {
            this.userManager = userManager;
            this.studentRepository = studentRepository;
            this.programmeRepository = programmeRepository;
            this.assessmentResultRepository = assessmentResultRepository;
        }

        [Authorize(Roles = "Admin")]
        // GET: Student/RegisterStudent
        public async Task<IActionResult> Register()
        {
            try
            {
                ViewBag.ProgrammeList = await programmeRepository.AllAsync();
                ViewBag.StudentId = GetStudentId();
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
        // GET: Student/RegisterStudent
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student, IFormFile image)
        {
            try
            {
                if (image != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        image.CopyTo(stream);
                        student.StudentImage = stream.ToArray();
                    }
                }

                if (ModelState.IsValid)
                {
                    await studentRepository.AddAsync(student);

                    //Create Student Role for Authentication
                    string fullName = student.FirstName + " " + student.SurName;
                    await CreateStudentAsUserAsync(student.StudentID, student.Password, fullName);

                    // Commit changes 
                    await studentRepository.SaveChangesAsync();

                    ModelState.Clear();

                    ViewBag.SuccessMsg = "Student Successfully Registered";

                    // Redirect to different view to show newly created student
                    return RedirectToAction("Details", new { id = student.StudentID });
                }
                return View(student);
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

        private async Task CreateStudentAsUserAsync(string studentID, string password, string fullName)
        {
            AppUser user = new AppUser();
            user.UserName = studentID;
            user.FullName = fullName;
            var result = await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "Student");
        }

        [Authorize(Roles = "Admin")]
        // GET: Student
        public async Task<IActionResult> ShowAll()
        {
            try
            {
                return View(await studentRepository.AllIncludeProgrammeAsync());
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

        [Authorize(Roles = "Admin, Student")]
        [HttpGet] // Show result of a particular Student
        public async Task<IActionResult> ShowResult(string id)
        {
            try
            {
                var student = await studentRepository.FindByIdAsync(id);

                if (student == null)
                {
                    ViewBag.ErrorMsg = $"Could not find student: {id}";
                    return RedirectToAction("Unknown", "Error");
                }

                var assessmentResults = await assessmentResultRepository.GetStudentAssessmentResults(id);

                ShowStudentResultViewModel vm = new ShowStudentResultViewModel();
                vm.StudentName = student.FirstName + " " + student.SurName;
                vm.Programme = student.Programme.ProgrammeName;
                vm.StudentImage = student.StudentImage;
                vm.listOfAssessmentResult = assessmentResults.ToList();

                return View(vm);
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

        [Authorize(Roles = "Admin, Student")]
        [HttpGet] // GET: Student/Details/5
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMsg = $"Invalid student ID: {id}";
                    return RedirectToAction("Unknown", "Error");
                }

                var student = await studentRepository.FindByIdIncludeProgrammeAsync(id);
                if (student == null)
                {
                    ViewBag.ErrorMsg = $"Could not find student: {id}";
                    return RedirectToAction("NotFoundPage", "Error");
                }

                return View(student);
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
        [HttpGet] // GET: Student/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await studentRepository.FindByIdIncludeProgrammeAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, 
            [Bind("StudentID,FirstName,SurName,Password,AddressOne,AddressTwo,Town,County,MobilePhoneNumber,EmailAddress,EmergencyMobilePhoneNumber,StudentPPS,ProgrammeFeePaid,DateOfBirth,GenderType,FullOrPartTime,StudentImage,ProgrammeID")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    studentRepository.Update(student);
                    await studentRepository.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = student.StudentID });
                }
                catch (DataAccessException e)
                {
                    if (!await StudentExists(student.StudentID))
                    {
                        return NotFound();
                    }
                    ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                    return RedirectToAction("Unknown", "Error");
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                    return RedirectToAction("Unknown", "Error");
                }
            }
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrorMsg = $"Invalid student ID: {id}";
                    return RedirectToAction("Unknown", "Error");
                }

                var student = await studentRepository.FindByIdAsync(id);
                if (student == null)
                {
                    ViewBag.ErrorMsg = $"Could not find student: {id}";
                    return RedirectToAction("Unknown", "Error");
                }

                return View(student);
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
        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await studentRepository.DeleteStudentAsync(id);
                await userManager.DeleteAsync(new AppUser {UserName = id });
                await studentRepository.SaveChangesAsync();
                
                return RedirectToAction(nameof(ShowAll));
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

        private string GetStudentId()
        {
            var r = new Random();
            return "S" + r.Next(100_000, 999_999).ToString();
        }
        private async Task<bool> StudentExists(string Id)
        {
            var student = await studentRepository.FindAsync(s => s.StudentID == Id);
            return (student != null) ? true : false;
        }
    }
}