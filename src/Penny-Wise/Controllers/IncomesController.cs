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
    public class IncomesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncomesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _context.Transaction.Include("Account").Include("Category").Where(t => t.Type).ToListAsync();
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
        public async Task<IActionResult> AddNew([Bind("ID,Type,Value,Category")] Transaction income)
        {
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
