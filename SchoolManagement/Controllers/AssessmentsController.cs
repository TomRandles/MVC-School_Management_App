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
    [Authorize(Roles = "Admin")]
    public class AssessmentsController : Controller
    {
        private readonly IAssessmentRepository assessmentRepository;
        private readonly IModuleRepository moduleRepository;

        public AssessmentsController(IAssessmentRepository assessmentRepository,
                                     IModuleRepository moduleRepository)
        {
            this.assessmentRepository = assessmentRepository;
            this.moduleRepository = moduleRepository;
        }

        // GET: Assessments
        public async Task<IActionResult> Index()
        {
            return View(await assessmentRepository.AllIncludeModuleAsync());
        }

        // GET: Assessments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessment = await assessmentRepository.FindByIdAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // GET: Assessments/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.ModuleList = await moduleRepository.AllAsync();
                ViewBag.AssessmentID = await GetAssessmentID();
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
        private async Task<string> GetAssessmentID()
        {
            string assessmentID = "";
            var assessment = await assessmentRepository.FindByIdAsync(assessmentID);
            if (assessment == null)
            // while (_context.Assessments.Any(m => m.AssessmentID == assessmentID) || assessmentID == "")
            {
                Random r = new Random();
                assessmentID = "M" + r.Next(100_00, 999_99).ToString();
            }
            return assessmentID;
        }

        // POST: Assessments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Assessment assessment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await assessmentRepository.AddAsync(assessment);
                    await assessmentRepository.SaveChangesAsync();

                    ViewBag.SuccessMsg = "Assessment Created Successfully";
                    ViewBag.ModuleList = await moduleRepository.AllAsync();
                    ViewBag.AssessmentID = await GetAssessmentID();
                    ModelState.Clear();
                    return View();
                }
                ViewBag.ModuleList = await moduleRepository.AllAsync();
                return View(assessment);
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

        // GET: Assessments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.ModuleList = await moduleRepository.AllAsync();
            var assessment = await assessmentRepository.FindByIdAsync(id); 
            if (assessment == null)
            {
                return NotFound();
            }
            return View(assessment);
        }

        // POST: Assessments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, 
                                              [Bind("AssessmentID,AssessmentName,AssessmentDescription,AssessmentTotalMark,AssessmentType,ModuleID")] Assessment assessment)
        {
            if (id != assessment.AssessmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    assessmentRepository.Update(assessment);
                    await assessmentRepository.SaveChangesAsync();
                }
                catch (DataAccessException e)
                {
                    if (!AssessmentExists(assessment.AssessmentID))
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

                return RedirectToAction(nameof(Index));
            }
            return View(assessment);
        }

        // GET: Assessments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            var assessment = await assessmentRepository.FindByIdAsync(id); 
            // _context.Assessments
            //    .FirstOrDefaultAsync(m => m.AssessmentID == id);
            if (assessment == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
            return View(assessment);
        }

        // POST: Assessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await assessmentRepository.DeleteAssessmentAsync(id);
                await assessmentRepository.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DataAccessException e)
            {
                var errorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Delete", "Error", errorMsg);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }
        private bool AssessmentExists(string id)
        {
            var assessment = assessmentRepository.FindAsync(a => a.AssessmentID == id);
            return (assessment != null) ? true : false;
        }
    }
}