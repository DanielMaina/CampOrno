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
    [Authorize(Roles = "Admin,Supervisor,Staff")]
    public class CamperActivitiesController : Controller
    {
        private readonly CampOrnoContext _context;

        public CamperActivitiesController(CampOrnoContext context)
        {
            _context = context;
        }

        // GET: CamperActivities
        public async Task<IActionResult> Index(int? CamperID, string actionButton,
            string sortDirection = "desc", string sortField = "Activity")
        {
            if (!CamperID.HasValue)
            {
                //Can't work without a Camper so go back to Camper Index
                return RedirectToAction("Index", "Campers");
            }
            PopulateDropDownLists();

            var camperActivities =from c in _context.CamperActivities
                                  .Include(c => c.Activity)
                                  .Where(c=>c.CamperID==CamperID)
                                  select c;

            //Before we sort, see if we have called for a change of sorting
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
            }
            //Now we know which field and direction to sort by
            if (sortField == "Fee")
            {
                if (sortDirection == "asc")
                {
                    camperActivities = camperActivities
                        .OrderBy(p => p.Fee)
                        .ThenBy(p => p.Activity.Name);
                }
                else
                {
                    camperActivities = camperActivities
                       .OrderByDescending(p => p.Fee)
                       .ThenBy(p => p.Activity.Name);
                }
            }
            else if (sortField.Contains("Number"))
            {
                if (sortDirection == "asc")
                {
                    camperActivities = camperActivities
                        .OrderBy(p => p.NumberOfSessions)
                       .ThenBy(p => p.Activity.Name);
                }
                else
                {
                    camperActivities = camperActivities
                        .OrderByDescending(p => p.NumberOfSessions)
                       .ThenBy(p => p.Activity.Name);
                }
            }
            else //Activity
            {
                if (sortDirection == "asc")
                {
                    camperActivities = camperActivities
                        .OrderBy(p => p.Activity.Name);
                }
                else
                {
                    camperActivities = camperActivities
                        .OrderByDescending(p => p.Activity.Name);
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //Now get the MASTER record, the camper, so it can be displayed at the top of the screen
            Camper camper = _context.Campers
                .Include(c => c.Compound)
                .Include(c => c.Counselor)
                .Include(c => c.CamperDiets)
                .ThenInclude(cd => cd.DietaryRestriction)
                .Where(p => p.ID == CamperID.GetValueOrDefault())
                .FirstOrDefault();
            ViewBag.Camper = camper;

            return View(await camperActivities.ToListAsync());
        }

        // GET: CamperActivities/ADD
        public IActionResult Add(int? CamperID, string CamperName)
        {
            if (!CamperID.HasValue)
            {
                return RedirectToAction("Index", "Campers");
            }
            ViewData["CamperName"] = CamperName;
            CamperActivity a = new CamperActivity()
            {
                CamperID = CamperID.GetValueOrDefault()
            };
            PopulateDropDownLists();
            return View(a);
        }

        // POST: CamperActivities/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("ID,Fee,NumberOfSessions,CamperID,ActivityID")] CamperActivity camperActivity, string CamperName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(camperActivity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { camperActivity.CamperID });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.ToUpper().Contains("UNIQUE"))
                {
                    ModelState.AddModelError("ActivityID", "Unable to save changes. Remember, a camper canot be in an activity more then once.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            PopulateDropDownLists(camperActivity);
            ViewData["CamperName"] = CamperName;
            return View(camperActivity);
        }

        // GET: CamperActivities/Update/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camperActivity = await _context.CamperActivities
                .Include(c=>c.Activity)
                .Include(c=>c.Camper)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == id);
            if (camperActivity == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(camperActivity);
            return View(camperActivity);
        }

        // POST: CamperActivities/Update/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            var camperActivityToUpdate = await _context.CamperActivities
                .Include(c => c.Activity)
                .Include(c => c.Camper)
                .FirstOrDefaultAsync(c => c.ID == id);
            if (camperActivityToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<CamperActivity>(camperActivityToUpdate, "",
                p => p.NumberOfSessions, p => p.Fee, p => p.ActivityID))
            {
                try
                {
                    _context.Update(camperActivityToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", new { camperActivityToUpdate.CamperID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamperActivityExists(camperActivityToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException dex)
                {
                    if (dex.InnerException.Message.ToUpper().Contains("UNIQUE"))
                    {
                        ModelState.AddModelError("ActivityID", "Unable to save changes. Remember, a camper canot be in an activity more then once.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            PopulateDropDownLists(camperActivityToUpdate);
            return View(camperActivityToUpdate);
        }

        // GET: CamperActivities/Remove/5
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camperActivity = await _context.CamperActivities
                .Include(c => c.Activity)
                .Include(c => c.Camper)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == id);
            if (camperActivity == null)
            {
                return NotFound();
            }

            return View(camperActivity);
        }

        // POST: CamperActivities/Remove/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var camperActivity = await _context.CamperActivities
                .Include(c => c.Activity)
                .Include(c => c.Camper)
                .FirstOrDefaultAsync(c => c.ID == id);
            try
            {
                _context.CamperActivities.Remove(camperActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { camperActivity.CamperID });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(camperActivity);
        }

        private void PopulateDropDownLists(CamperActivity camperActivity = null)
        {
            ViewData["ActivityID"] = new SelectList(_context.Activities
                .OrderBy(a=>a.Name), "ID", "Name", camperActivity?.ActivityID);
        }
        private bool CamperActivityExists(int id)
        {
            return _context.CamperActivities.Any(e => e.ID == id);
        }
    }
}
