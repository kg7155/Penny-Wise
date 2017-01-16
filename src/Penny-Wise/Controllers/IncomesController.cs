using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Penny_Wise.Data;
using Penny_Wise.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;

namespace Penny_Wise.Controllers
{
    [Authorize]
    public class IncomesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncomesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            var items = await _context.Transaction.Include(o => o.Account).Include(o => o.Category).Where(t => t.Type).ToListAsync();
            return View(items);
        }

        // GET: Incomes/AddNew
        public IActionResult AddNew()
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            return View();
        }

        // POST: Incomes/AddNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew([Bind("ID,Type,Value,Category,Date")] Transaction income)
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            var allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                income.Type = true;

                int accountId = int.Parse(Request.Form["incomes-select-account"]);
                var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
                income.Account = account;
                
                int categoryId = int.Parse(Request.Form["incomes-select-category"]);
                var category = await _context.Categories.SingleOrDefaultAsync(a => a.ID == categoryId);
                income.Category = category;

                _context.Add(income);

                income.Account.Balance += income.Value;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(income);
        }

        // GET: Incomes/AddNewCategory
        public IActionResult AddNewCategory()
        {
            return View();
        }

        // POST: Incomes/AddNewCategory
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

        // GET: Incomes/Edit/5
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

            var income = await _context.Transaction.SingleOrDefaultAsync(m => m.ID == id);
            if (income == null)
            {
                return NotFound();
            }
            return View(income);
        }

        // POST: Incomes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Type,Value,Category,Date")] Transaction income)
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            if (id != income.ID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    //var oldIncome = _context.Transaction.First(t => t.ID == income.ID);
                    //var oldValue = _context.Entry(income).Property("Value").OriginalValue;
                    
                    income.Type = true;
                    _context.Update(income);

                    int accountId = Int32.Parse(Request.Form["incomes-select-account"]);
                    var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
                    income.Account = account;
                    //income.Account.Balance -= oldValue - income.Value;

                    int categoryId = int.Parse(Request.Form["incomes-select-category"]);
                    var category = await _context.Categories.SingleOrDefaultAsync(a => a.ID == categoryId);
                    income.Category = category;
                    
                    await _context.SaveChangesAsync();
                    await RecalculateAccountBalance(accountId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(income.ID))
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
            return View(income);
        }

        // GET: Incomes/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Transaction.Include(o => o.Account).Include(o => o.Category).SingleOrDefaultAsync(m => m.ID == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            var income = await _context.Transaction.SingleOrDefaultAsync(m => m.ID == id);
            income.Account.Balance -= income.Value;

            _context.Transaction.Remove(income);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Incomes/Graph
        [HttpGet]
        public string Graph(int accountId)
        {
            int numDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            List<double> data = new List<double>();

            for (var i = 1; i <= numDays; i++)
            {
                double value = 0;
                var incomes = _context.Transaction.Where(o => o.Type && o.Account.ID == accountId && o.Date.Year == DateTime.Now.Year && o.Date.Month == DateTime.Now.Month && o.Date.Day == i);
                foreach (var income in incomes)
                {
                    value += income.Value;
                }
                data.Add(value);
            }
            return JsonConvert.SerializeObject(data);
        }

        private bool IncomeExists(int id)
        {
            return _context.Transaction.Any(e => e.ID == id);
        }

        private async Task<int> RecalculateAccountBalance(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);
            double balance = account.Balance;
            
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

            account.Balance = balance;
            await _context.SaveChangesAsync();

            return 0;
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
