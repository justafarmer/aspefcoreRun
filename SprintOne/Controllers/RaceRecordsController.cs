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
        {
            var viewModel = new RaceRecordViewModel();

//          ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName");
            return View(viewModel);
        }

        // POST: RaceRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaceType,RaceTimeHours,RaceTimeMinutes,RaceTimeSeconds")] RaceRecordViewModel raceRecordView)
        {

            
            if (ModelState.IsValid)
            {
                var currid = GetUserID();
                int totalTime = raceRecordView.RaceTimeHours * 3600 + raceRecordView.RaceTimeMinutes * 60 + raceRecordView.RaceTimeSeconds;
                RaceRecord race = new RaceRecord();
                race.ProfileID = currid;
                race.RaceTime = totalTime;
                race.RaceType = raceRecordView.RaceType;

                int mileTime;
                switch (raceRecordView.RaceType)
                {
                    case 1:
                        mileTime = totalTime;
                        break;
                    case 2:
                        mileTime = Convert.ToInt32(totalTime / 3.106);
                        break;
                    case 3:
                        mileTime = Convert.ToInt32(totalTime / 6.21);
                        break;
                    case 4:
                        mileTime = Convert.ToInt32(totalTime / 13.11);
                        break;
                    case 5:
                        mileTime = Convert.ToInt32(totalTime / 26.22);
                        break;
                    default:
                        mileTime = 0;
                        break;
                }
                race.MileTime = mileTime;
                try
                {
                    _context.Add(race);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View();
                }

            }
//            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName", raceRecord.ProfileID);
            return View(raceRecordView);
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

        public int GetUserID()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.Find(userId);
            var currid = user.UserID;
            return currid;
        }

        private bool RaceRecordExists(int id)
        {
            return _context.RaceRecords.Any(e => e.RaceRecordID == id);
        }
    }
}
