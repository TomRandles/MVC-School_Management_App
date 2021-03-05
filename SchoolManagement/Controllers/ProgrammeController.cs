using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Utilities;
using ServicesLib.Domain.Models;
using ServicesLib.Domain.Utilities;
using ServicesLib.Services.Repositories;
using ServicesLib.Services.Repository.Generic;

namespace SchoolManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProgrammeController : Controller
    {
        private readonly IProgrammeRepository programmeRepository;
        public ProgrammeController(IProgrammeRepository programmeRepository)
        {
            this.programmeRepository = programmeRepository;
        }

        // GET: Programme
        public async Task<IActionResult> Index()
        {
            return View(await programmeRepository.AllAsync());
        }

        [HttpGet]// GET: Programme/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMsg = $"Invalid programme ID: {id}";
                return NotFound();
            }

            var programme = await programmeRepository.FindByIdAsync(id);
            if (programme == null)
            {
                ViewBag.ErrorMsg = $"Invalid programme {id}";
                return NotFound();
            }
            return View(programme);
        }

        // GET: Programme/Create
        public IActionResult Create()
        {
            ViewBag.ProgrammeID = GetProgrammeID();
            return View();
        }

        // POST: Programme/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgrammeID,ProgrammeName,ProgrammeDescription,ProgrammeQQILevel,ProgrammeCredits,ProgrammeCost")] Programme programme)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await programmeRepository.AddAsync(programme);
                    await programmeRepository.SaveChangesAsync();

                    ViewBag.SuccessMsg = "Programme Created Successfully";

                    var programmeDetails = await programmeRepository.FindByIdAsync(programme.ProgrammeID);

                    ModelState.Clear();

                    return RedirectToAction("Details", new { id = programme.ProgrammeID });
                }
                return View(programme);

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

        // GET: Programme/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMsg = $"Invalid programme ID: {id}";
                return NotFound();
            }
            var programme = await programmeRepository.FindByIdAsync(id);
            if (programme == null)
            {
                return NotFound();
            }
            return View(programme);
        }

        // POST: Programme/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProgrammeID,ProgrammeName,ProgrammeDescription,ProgrammeQQILevel,ProgrammeCredits,ProgrammeCost")] Programme programme)
        {
            if (id != programme.ProgrammeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    programmeRepository.Update(programme);
                    await programmeRepository.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = programme.ProgrammeID });
                }
                catch (DataAccessException e)
                {
                    if (!ProgrammeExists(programme.ProgrammeID))
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
            return View(programme);
        }

        [HttpGet] // GET: Programme/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                ViewBag.ErrorMsg = $"Invalid programme ID: {id}";
                return RedirectToAction("NotFoundPage", "Error");
            }
            var programme = await programmeRepository.FindByIdAsync(id);
            if (programme == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            return View(programme);
        }

        // POST: Programme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await programmeRepository.DeleteProgrammeAsync(id);
                await programmeRepository.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DataAccessException e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("Data Access exception. ", e);
                return RedirectToAction("Delete", "Error", new { errorMsg = ViewBag.ErrorMsg });
            }
            catch (Exception e)
            {
                ViewBag.ErrorMsg = ErrorProcessing.ProcessException("General exception. ", e);
                return RedirectToAction("Unknown", "Error");
            }
        }
        private bool ProgrammeExists(string id)
        {
            var programme = programmeRepository.FindAsync(e => e.ProgrammeID == id);
            return (programme != null) ? true : false;
        }
        private string GetProgrammeID()
        {
            Random r = new Random();
            return "P" + r.Next(100_00, 999_99).ToString();
        }
    }
}