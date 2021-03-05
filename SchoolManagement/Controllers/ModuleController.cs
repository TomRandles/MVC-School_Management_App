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
    public class ModuleController : Controller
    {
        private readonly IModuleRepository moduleRepository;
        private readonly IProgrammeRepository programmeRepository;
        public ModuleController(IModuleRepository moduleRepository, IProgrammeRepository programmeRepository)
        {
            this.moduleRepository = moduleRepository;
            this.programmeRepository = programmeRepository;
        }

        [HttpGet]// GET: Module
        public async Task<IActionResult> Index()
        {
            return View(await moduleRepository.AllIncludeProgramme());
        }

        [HttpGet] // GET: Module/Details/5
        public async Task<IActionResult> Details(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var module = await moduleRepository.FindByIdAsync(Id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }


        [HttpGet]// GET: Module/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.ProgrammeList = await programmeRepository.AllAsync();
                ViewBag.ModuleID = GetModuleId();
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

        // POST: Module/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Module module)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await moduleRepository.AddAsync(module);
                    await moduleRepository.SaveChangesAsync();

                    ViewBag.SuccessMsg = "Module Created Successfully";

                    ModelState.Clear();

                    return RedirectToAction("Details", new { id = module.ModuleID });                    
                }
                return View(module);
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

        [HttpGet]// GET: Module/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.ProgrammeList = programmeRepository.AllAsync();
            var module = await moduleRepository.FindByIdAsync(id);

            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // POST: Module/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ModuleID,ModuleName,ModuleDescription,ModuleCredits")] Module @module)
        {
            if (id != module.ModuleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    moduleRepository.Update(module);
                    await moduleRepository.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = module.ModuleID });
                }
                catch (DataAccessException e)
                {
                    if (!await ModuleExists(module.ModuleID))
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
            return View(module);
        }

        [HttpGet] // GET: Module/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }

            var module = await moduleRepository.FindByIdAsync(id);
            if (module == null)
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
            return View(module);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await moduleRepository.DeleteModuleAsync(id);
                await moduleRepository.SaveChangesAsync();
                
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
        private async Task<bool> ModuleExists(string id)
        {
            var module = await moduleRepository.FindAsync(m => m.ModuleID == id);
            return (module != null) ? true : false; 
        }
        private string GetModuleId()
        {
            var r = new Random();
            return "M" + r.Next(1000_0, 999_99).ToString();
        }
    }
}