using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Penny_Wise.Data;
using Penny_Wise.Models;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace Penny_Wise.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GoalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Goals.Include("Account").ToListAsync();
            return View(items);
        }

        // GET: Goals/AddNew
        public IActionResult AddNew()
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;
            return View();
        }

        // POST: Goals/AddNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew([Bind("ID,Amount,DateTo,Account")] Goal goal)
        {
            if (ModelState.IsValid)
            {
                int accountId = int.Parse(Request.Form["goals-select-account"]);
                var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
                goal.Account = account;
                _context.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals.SingleOrDefaultAsync(m => m.ID == id);
            if (goal == null)
            {
                return NotFound();
            }
            return View(goal);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Amount,DateTo")] Goal goal)
        {
            if (id != goal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goal);

                    int accountId = Int32.Parse(Request.Form["goals-select-account"]);
                    var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
                    goal.Account = account;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalExists(goal.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        // GET: Goals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals.Include(o => o.Account).SingleOrDefaultAsync(m => m.ID == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goal = await _context.Goals.SingleOrDefaultAsync(m => m.ID == id);
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GoalExists(int id)
        {
            return _context.Goals.Any(e => e.ID == id);
        }

        public double AmountLeft(int accountId, double amount)
        {
            double balance = 0;

            var incomes = _context.Transaction.Where(o => o.Account.ID == accountId && o.Type);
            foreach (var income in incomes)
            {
                balance += income.Value;
            }

            var expenses = _context.Transaction.Where(o => o.Account.ID == accountId && !o.Type);
            foreach (var expense in expenses)
            {
                balance -= expense.Value;
            }

            double amountLeft = amount - balance;

            if (amountLeft < 0.0)
                return 0.0;
            else
                return amount - balance;
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
