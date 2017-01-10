using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Penny_Wise.Data;
using Penny_Wise.Models;
using Microsoft.EntityFrameworkCore;

namespace Penny_Wise.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _context.Transaction.Include("Account").Include("Category").Where(t => !t.Type).ToListAsync();
            return View(items);
        }

        // GET: Expenses/AddNew
        public IActionResult AddNew()
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            return View();
        }

        // POST: Expenses/AddNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew([Bind("ID,Type,Value,Category,Date")] Transaction expense)
        {
            if (ModelState.IsValid)
            {
                expense.Type = false;

                int accountId = int.Parse(Request.Form["expenses-select-account"]);
                var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
                expense.Account = account;

                int categoryId = int.Parse(Request.Form["expenses-select-category"]);
                var category = await _context.Categories.SingleOrDefaultAsync(a => a.ID == categoryId);
                expense.Category = category;

                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        // GET: Expenses/AddNewCategory
        public IActionResult AddNewCategory()
        {
            return View();
        }

        // POST: Expenses/AddNewCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCategory([Bind("ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddNew");
            }
            return View(category);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Transaction.SingleOrDefaultAsync(m => m.ID == id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Type,Value,Category,Date")] Transaction expense)
        {
            if (id != expense.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);

                    expense.Type = false;

                    int accountId = Int32.Parse(Request.Form["incomes-select-account"]);
                    var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
                    expense.Account = account;

                    int categoryId = int.Parse(Request.Form["incomes-select-category"]);
                    var category = await _context.Categories.SingleOrDefaultAsync(a => a.ID == categoryId);
                    expense.Category = category;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(expense.ID))
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
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Transaction.Include("Account").Include("Category").SingleOrDefaultAsync(m => m.ID == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Transaction.SingleOrDefaultAsync(m => m.ID == id);
            _context.Transaction.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool IncomeExists(int id)
        {
            return _context.Transaction.Any(e => e.ID == id);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
