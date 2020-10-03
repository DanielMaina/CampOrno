using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampOrno.Data;
using CampOrno.Models;
using Microsoft.AspNetCore.Authorization;

namespace CampOrno.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly CampOrnoContext _context;

        public ActivitiesController(CampOrnoContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activities.ToListAsync());
        }

        // GET: Activities/Details/5
        [Authorize(Roles = "Admin,Supervisor,Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a=>a.CamperActivities)
                .ThenInclude(a=>a.Camper)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Admin,Supervisor,Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor,Staff")]
        public async Task<IActionResult> Create([Bind("ID,Name")] Activity activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(activity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Admin,Supervisor,Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor,Staff")]
        public async Task<IActionResult> Edit(int id)
        {
            var activityToUpdate = await _context.Activities.FindAsync(id);
            if (activityToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Activity>(activityToUpdate, "",
                p => p.Name))
            {
                try
                {
                    _context.Update(activityToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activityToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(activityToUpdate);
        }

        // GET: Activities/Delete/5

        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            try
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.Contains("FK_CamperActivities_Activities_ActivityID"))
                {
                    ModelState.AddModelError("", "Unable to save changes. Remember, you cannot delete an activity that any campers have.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(activity);
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.ID == id);
        }
    }
}
