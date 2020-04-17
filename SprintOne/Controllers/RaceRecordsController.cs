using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SprintOne.Data;
using SprintOne.Models;
using SprintOne.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SprintOne.Controllers
{
    [Authorize]
    public class RaceRecordsController : Controller
    {
        private readonly MatchContext _context;
        private readonly UserManager<User> _userManager;

        public RaceRecordsController(MatchContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RaceRecords
        public async Task<IActionResult> Index()
        {
            var matchContext = _context.RaceRecords.Include(r => r.RaceProfile);
            return View(await matchContext.ToListAsync());
            //return View(await _context.RaceRecords.ToListAsync());
        }

        public async Task<IActionResult> IndexMatch(int? lower, int? upper)
        {
            IQueryable<PaceMatchViewModel> q =
                    from x in _context.RaceRecords
                    orderby x.ProfileID descending
                    group x by x.ProfileID into y
                    select new PaceMatchViewModel()
                    {
                        RunnerID = y.Key,
                        MileTime = y.Min(m => m.MileTime)
                    }; 

            return View(await q.AsNoTracking().ToListAsync());
        }



        // GET: RaceRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceRecord = await _context.RaceRecords
                .Include(r => r.RaceProfile)
                .FirstOrDefaultAsync(m => m.RaceRecordID == id);
            if (raceRecord == null)
            {
                return NotFound();
            }

            return View(raceRecord);
        }

        // GET: RaceRecords/Create
        public IActionResult Create()
        {/*
            var viewModel = new RaceRecord();

            viewModel.RaceLength = new List<SelectListItem>
            {
                new SelectListItem{Text = "One Mile", Value = "1"},
                new SelectListItem{Text = "5k", Value = "2"},
                new SelectListItem{Text = "10k", Value = "3"},
                new SelectListItem{Text = "Half Marathon", Value = "4"},
                new SelectListItem{Text = "Full Marathon", Value = "5"},
            };
            */
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName");
            return View();
        }

        // POST: RaceRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceType,RaceTime,MileTime")] RaceRecord raceRecord)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.Find(userId);
            var currid = user.UserID;



            if (ModelState.IsValid)
            {
                raceRecord.ProfileID = currid;
                _context.Add(raceRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName", raceRecord.ProfileID);
            return View(raceRecord);
        }

        // GET: RaceRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceRecord = await _context.RaceRecords.FindAsync(id);
            if (raceRecord == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName", raceRecord.ProfileID);
            return View(raceRecord);
        }

        // POST: RaceRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceRecordID,UserID,RaceType,RaceTime,MileTime")] RaceRecord raceRecord)
        {
            if (id != raceRecord.RaceRecordID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raceRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceRecordExists(raceRecord.RaceRecordID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName", raceRecord.ProfileID);
            return View(raceRecord);
        }

        // GET: RaceRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raceRecord = await _context.RaceRecords
                .Include(r => r.RaceProfile)
                .FirstOrDefaultAsync(m => m.RaceRecordID == id);
            if (raceRecord == null)
            {
                return NotFound();
            }

            return View(raceRecord);
        }

        // POST: RaceRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raceRecord = await _context.RaceRecords.FindAsync(id);
            _context.RaceRecords.Remove(raceRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceRecordExists(int id)
        {
            return _context.RaceRecords.Any(e => e.RaceRecordID == id);
        }
    }
}
