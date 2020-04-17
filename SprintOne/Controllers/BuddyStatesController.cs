using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SprintOne.Data;
using SprintOne.Models;

namespace SprintOne.Controllers
{
    public class BuddyStatesController : Controller
    {
        private readonly MatchContext _context;

        public BuddyStatesController(MatchContext context)
        {
            _context = context;
        }

        // GET: BuddyStates
        public async Task<IActionResult> Index()
        {
            var matchContext = _context.BuddyList.Include(b => b.FirstProfile).Include(b => b.SecondProfile);
            return View(await matchContext.ToListAsync());
        }

        // GET: BuddyStates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buddyState = await _context.BuddyList
                .Include(b => b.FirstProfile)
                .Include(b => b.SecondProfile)
                .FirstOrDefaultAsync(m => m.FirstID == id);
            if (buddyState == null)
            {
                return NotFound();
            }

            return View(buddyState);
        }

        // GET: BuddyStates/Create
        public IActionResult Create()
        {
            ViewData["FirstID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName");
            ViewData["SecondID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName");
            return View();
        }

        // POST: BuddyStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstID,SecondID,Status")] BuddyState buddyState)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buddyState);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", buddyState.FirstID);
            ViewData["SecondID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", buddyState.SecondID);
            return View(buddyState);
        }

        // GET: BuddyStates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buddyState = await _context.BuddyList.FindAsync(id);
            if (buddyState == null)
            {
                return NotFound();
            }
            ViewData["FirstID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", buddyState.FirstID);
            ViewData["SecondID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", buddyState.SecondID);
            return View(buddyState);
        }

        // POST: BuddyStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstID,SecondID,Status")] BuddyState buddyState)
        {
            if (id != buddyState.FirstID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buddyState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuddyStateExists(buddyState.FirstID))
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
            ViewData["FirstID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", buddyState.FirstID);
            ViewData["SecondID"] = new SelectList(_context.Profiles, "ProfileID", "FirstName", buddyState.SecondID);
            return View(buddyState);
        }

        // GET: BuddyStates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buddyState = await _context.BuddyList
                .Include(b => b.FirstProfile)
                .Include(b => b.SecondProfile)
                .FirstOrDefaultAsync(m => m.FirstID == id);
            if (buddyState == null)
            {
                return NotFound();
            }

            return View(buddyState);
        }

        // POST: BuddyStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buddyState = await _context.BuddyList.FindAsync(id);
            _context.BuddyList.Remove(buddyState);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuddyStateExists(int id)
        {
            return _context.BuddyList.Any(e => e.FirstID == id);
        }
    }
}
