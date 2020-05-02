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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        // GET: RaceRecords/Create
        public IActionResult CreatePersonal()
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
        public async Task<IActionResult> CreatePersonal([Bind("RaceType,RaceTimeHours,RaceTimeMinutes,RaceTimeSeconds")] RaceRecordViewModel raceRecordView)
        {
            if (ModelState.IsValid)
            {
                var currid = GetUserID();
                int totalTime = raceRecordView.RaceTimeHours * 3600 + raceRecordView.RaceTimeMinutes * 60 + raceRecordView.RaceTimeSeconds;
                int mileTime = Functions.GetMileTime(totalTime, raceRecordView.RaceType);
                if (mileTime > 223)
                {
                    var race = new RaceRecord
                    {
                        ProfileID = currid,
                        RaceTime = totalTime,
                        RaceType = raceRecordView.RaceType,
                        MileTime = mileTime
                    };


                    try
                    {
                        _context.Add(race);
                        await _context.SaveChangesAsync();
                        //return RedirectToAction("Index", "Profiles", new {@show = "myracerecords"});
                        return RedirectToAction("CreatePersonalSuccess", "RaceRecords", race);
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wow, you must be fast!  Unfortnately your mile time must be greater than 3:43!");
                }
            }
//            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName", raceRecord.ProfileID);
            return View(raceRecordView);
        }

        // GET: RaceRecords/Edit/5
        public async Task<IActionResult> EditPersonal(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var race = await _context.RaceRecords.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }

            var hours = race.RaceTime / 3600;
            var minutes = (race.RaceTime % 3600) / 60;
            var seconds = ((race.RaceTime % 3600) % 60);

            var viewModel = new RaceRecordViewModel()
            {
                Record = race,
                RaceTimeHours = hours,
                RaceTimeMinutes = minutes,
                RaceTimeSeconds = seconds,
                RaceType = race.RaceType
            };

            return View(viewModel);
        }

        // POST: RaceRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonal(int? id, [Bind("RaceType,RaceTimeHours,RaceTimeMinutes,RaceTimeSeconds")] RaceRecordViewModel viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.RaceRecords.FindAsync(id);
            
            if (race == null)
            {
                return NotFound();
            }

            int totalTime = viewModel.RaceTimeHours * 3600 + viewModel.RaceTimeMinutes * 60 + viewModel.RaceTimeSeconds;
            int mileTime = Functions.GetMileTime(totalTime, viewModel.RaceType);
            if (mileTime > 223)
            {
                race.RaceTime = totalTime;
                race.MileTime = mileTime;
                race.RaceType = viewModel.RaceType;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(race);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RaceRecordExists(viewModel.Record.RaceRecordID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //return RedirectToAction(nameof(Index));
                    
                    return RedirectToAction("EditPersonalSuccess", "RaceRecords", race);
                }
            }
            else
            {
                viewModel.Record = race;
                ModelState.AddModelError("", "Wow, you must be fast!  Unfortnately your mile time must be greater than 3:43!");
            }

            return View(viewModel);
        }

        // GET: RaceRecords/Delete/5
        public async Task<IActionResult> DeletePersonal(int? id)
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
        [HttpPost, ActionName("DeletePersonal")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePersonalConfirmed(int id)
        {
            var raceRecord = await _context.RaceRecords.FindAsync(id);
            _context.RaceRecords.Remove(raceRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction("DeletePersonalSuccess", "RaceRecords");
        }


        public IActionResult CreatePersonalSuccess(RaceRecord viewModel)
        {
            return View(viewModel);
        }

        public IActionResult DeletePersonalSuccess(RaceRecord viewModel)
        {
            return View(viewModel);
        }

        public IActionResult EditPersonalSuccess(RaceRecord viewModel)
        {
            return View(viewModel);
        }


        //+++++ Default Actions +++++

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


        public IActionResult Create()
        {
            var viewModel = new RaceRecordViewModel();

            //          ViewData["UserID"] = new SelectList(_context.Users, "UserID", "FirstName");
            return View(viewModel);
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
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", raceRecord.ProfileID);
            return View(raceRecord);
        }


        // POST: RaceRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaceRecordID,ProfileID,RaceType,RaceTime,MileTime")] RaceRecord raceRecord)
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
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", raceRecord.ProfileID);
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
