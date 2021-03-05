using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.Domain.Models;
using ServicesLib.Domain.Utilities;
using ServicesLib.Services.Repositories;
using ServicesLib.Services.Repository.Generic;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class AssessmentResultsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IAssessmentResultRepository assessmentResultRepository;
        private readonly IProgrammeRepository programmeRepository;
        private readonly IModuleRepository moduleRepository;
        private readonly IAssessmentRepository assessmentRepository;

        public AssessmentResultsController(IStudentRepository studentRepository,
                                           IAssessmentResultRepository assessmentResultRepository,
                                           IProgrammeRepository programmeRepository,
                                           IModuleRepository moduleRepository,
                                           IAssessmentRepository assessmentRepository)
        {
            this.studentRepository = studentRepository;
            this.assessmentResultRepository = assessmentResultRepository;
            this.programmeRepository = programmeRepository;
            this.moduleRepository = moduleRepository;
            this.assessmentRepository = assessmentRepository;
        }

        [Authorize(Roles = "Admin, Teacher")]
        // GET: AssessmentResults/Create
        public async Task<IActionResult> Submit(string id)
        {
            try
            {
                var student = await studentRepository.FindByIdIncludeProgrammeAsync(id);
                if (student == null)
                {
                    ViewBag.StudentList = studentRepository.AllAsync(); 
                }
                else
                {
                    ViewBag.StudentId = student.StudentID;
                    ViewBag.StudentFullName = student.FirstName + " " + student.SurName;
                }
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

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(AssessmentResult assessmentResult)
        {
            try
            {
                string assessmentResultId = await GetAssessmentResultId();
                assessmentResult.AssessmentResultID = assessmentResultId;
                if (ModelState.IsValid)
                {
                    await assessmentResultRepository.AddAsync(assessmentResult);
                    await assessmentResultRepository.SaveChangesAsync();

                    return RedirectToAction(nameof(ShowAll));
                }

                //Student std = _context.Students
                //                .Include(m => m.Programme)
                //                .FirstOrDefault(m => m.StudentID == assessmentResult.StudentID);
                var student = await studentRepository.FindByIdIncludeProgrammeAsync(assessmentResult.StudentID);

                if (student == null)
                {
                    ViewBag.StudentList = await studentRepository.AllAsync();
                }
                else
                {
                    ViewBag.StudentId = student.StudentID;
                    ViewBag.StudentFullName = student.FirstName + " " + student.SurName;
                }
                return View(assessmentResult);
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
        private async Task<string> GetAssessmentResultId()
        {
            string studentID = "";
            var student = await studentRepository.FindByIdAsync(studentID);
            if (studentID == null)
            // while (_context.AssessmentResults.Any(m => m.StudentID == studentID) || studentID == "")
            {
                Random r = new Random();
                studentID = "AsRes" + r.Next(100_000, 999_999).ToString();
            }
            return studentID;
        }
        public async Task<JsonResult> GetProgrammeIdByStudent(string id)
        {
            // var student = _context.Students.Include(c => c.Programme).FirstOrDefault(m => m.StudentID == id);
            var student = await studentRepository.FindByIdIncludeProgrammeAsync(id);
            //var json = JsonConvert.SerializeObject(student);
            return Json(student);
        }

        public JsonResult GetModuleByProgramme(string id)
        {
            // var moduleList = _context.Modules.Where(m => m.ProgrammeID == id).ToList();
            var moduleList = moduleRepository.FindAllModulesForProgramme(id);
            return Json(moduleList);
        }


        public async Task<JsonResult> GetAssessmentByModule(string id)
        {
            var assessments = await assessmentRepository.AllAssessmentsForModule(id); 
            return Json(assessments);
        }

        public async Task<JsonResult> GetStudentById(string id)
        {
            var student = await studentRepository.FindByIdAsync(id);
            string fullName = default;
            if (student != null)
            {
                fullName= student.FirstName + " " + student.SurName;
            }
            return Json(fullName);
        }


        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> ShowAll()
        {
            ViewBag.programmeList = await programmeRepository.AllAsync();
            var assessmentResults = await assessmentResultRepository.GetAllAssessmentResults();
            return View(assessmentResults);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> ShowAll(string programmeID)
        {
            try
            {
                ViewBag.programmeList = await programmeRepository.AllAsync();
                var assessmentResults = assessmentResultRepository.AllResultsForProgrammeAsync(programmeID);
                return View(assessmentResults);
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
        public async Task<IActionResult> ShowAssessmentResult(string assessmentResultID)
        {
            var assessmentResult = await assessmentResultRepository.GetAssessmentResultDetailsAsync(assessmentResultID);
            return View(assessmentResult);            
        }

        // GET: AssessmentResults/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var assessmentResult = await assessmentResultRepository.GetAssessmentResultAsync(id);
            if (assessmentResult == null)
            {
                return NotFound();
            }
            return View(assessmentResult);
        }

        // GET: AssessmentResults/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var assessmentResult = await assessmentResultRepository.GetAssessmentResultAsync(id);
            if (assessmentResult == null)
            {
                return NotFound();
            }
            return View(assessmentResult);
        }

        // POST: AssessmentResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AssessmentResultID,AssessmentResultDescription,AssessmentResultMark,StudentID,ProgrammeID,AssessmentDate,ModuleID,AssessmentID")] AssessmentResult assessmentResult)
        {
            if (id != assessmentResult.AssessmentResultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    assessmentResultRepository.Update(assessmentResult);
                    await assessmentResultRepository.SaveChangesAsync();
                }

                catch (DataAccessException e)
                {
                    if (!AssessmentResultExists(assessmentResult.AssessmentResultID))
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

                return RedirectToAction(nameof(ShowAll));
            }
            return View(assessmentResult);
        }


        [Authorize(Roles = "Admin")]
        // GET: AssessmentResults/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            var assessmentResult = await assessmentResultRepository.GetAssessmentResultDetailsAsync(id);

            if (assessmentResult == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
            return View(assessmentResult);
        }

        [Authorize(Roles = "Admin")]
        // POST: AssessmentResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string assessmentResultID)
        {
            try
            {
                await assessmentResultRepository.DeleteAssessmentResultAsync(assessmentResultID);
                await assessmentResultRepository.SaveChangesAsync();

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
        private bool AssessmentResultExists(string id)
        {
            return assessmentResultRepository.AssessmentResultExists(id);
        }
    }
}