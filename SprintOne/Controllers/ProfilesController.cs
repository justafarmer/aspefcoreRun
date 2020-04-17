using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SprintOne.Models;
using SprintOne.Models.ViewModels;
using System.Security.Claims;
using SprintOne.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;


namespace SprintOne.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {

        private readonly MatchContext _context;
        private readonly UserManager<User> _userManager;

        public ProfilesController(MatchContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RaceRecords
        public async Task<IActionResult> AllProfile()
        {
            var profiles = _context.Profiles;

            return View(await profiles.ToListAsync());

        }

        public async Task<IActionResult> Index(string? show)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userName = User.FindFirstValue(ClaimTypes.Name);
            User user = _context.Users.Find(userId);
            var currid = user.UserID;

            var viewModel = new ProfileViewModel();

            viewModel.MyProfile =_context.Profiles.Find(currid);

            if (show != null)
            {
                if (show.Equals("myrelationships"))
                {
                    var relationshipList = _context.BuddyList
                        .Where(b => b.FirstID == currid || b.SecondID == currid)
                        .ToList();
                    viewModel.MyListFriends = new List<Profile>();
                    viewModel.MyListBlocked = new List<Profile>();
                    viewModel.MyListPending = new List<Profile>();
                    var relationID = 0;

                    foreach (var b in relationshipList)
                    {
                        if (b.FirstID == currid)
                        {
                            relationID = b.SecondID;
                        }
                        else
                        {
                            relationID = b.FirstID;
                        }

                        if (b.Status == 1)
                        {
                            viewModel.MyListFriends.Add(_context.Profiles.Find(relationID));
                        }
                        else if (b.Status == 2)
                        {
                            viewModel.MyListPending.Add(_context.Profiles.Find(relationID));
                        }
                        else if (b.Status == 3)
                        {
                            viewModel.MyListBlocked.Add(_context.Profiles.Find(relationID));
                        }
                        else
                        {
                            //Invalid status.
                        }
                    }
                }

                if (show.Equals("myracerecords"))
                {
                    viewModel.MyRaceRecords = await _context.RaceRecords
                        .Where(r => r.ProfileID == currid)
                        .OrderByDescending(t => t.MileTime)
                        .ToListAsync();
                }
            }
 
            return View(viewModel);

        }

            // GET: Users/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

            var user = new ProfileViewModel();
            user.MyProfile = await _context.Profiles
                    .Include(s => s.TimeEntries)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.ProfileID == id);

            user.MyRaceRecords = await _context.RaceRecords
                .Where(p => p.ProfileID == id)
                .ToListAsync();

            var status = _context.BuddyList
                .Where(b => (b.FirstID == 1 || b.SecondID == 1) && (b.FirstID == id || b.SecondID == id))
                .FirstOrDefault();

            if (status != null)
            {
                user.BuddyFlag = status.Status;
            }
            else
            {
                user.BuddyFlag = 0;
            }
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            // GET: Users/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Users/Create
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("FirstName,LastName")] User user)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Cannot save new User." +
                        "Try again, check that your entry information is correct.");
                }
                return View(user);
            }

            // GET: Users/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }

            // POST: Users/Edit/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("UserID,FirstName,LastName,CreationDate")] Profile user)
            {
                if (id != user.ProfileID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.ProfileID))
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
                return View(user);
            }

            // GET: Users/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Profiles
                    .FirstOrDefaultAsync(m => m.ProfileID == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }

            // POST: Users/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var user = await _context.Users.FindAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool UserExists(int id)
            {
                return _context.Profiles.Any(e => e.ProfileID == id);
            }

            public ViewResult AccessDenied()
                {
                    return View();
                }

    }

}
