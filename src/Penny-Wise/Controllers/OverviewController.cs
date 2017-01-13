using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Penny_Wise.Data;
using Penny_Wise.Models;

namespace Penny_Wise.Controllers
{
    [Authorize]
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task <IActionResult> Index()
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            List<DateTime> dates = _context.Transaction.Select(m => m.Date).Distinct().ToList();
            List<DateTime> distinctDates = new List<DateTime>();

            foreach (var date in dates)
            {
                if (distinctDates.FindIndex(x => x.Month == date.Month && x.Year == date.Year) < 0)
                {
                    distinctDates.Add(date);    
                }
            }
            ViewBag.Dates = distinctDates;
            
            var items = await _context.Transaction.Include(o => o.Account).Include(o => o.Category).ToListAsync();
            return View(items);
        }

        // GET: Overview/Chart
        [HttpGet]
        public string Chart(int accountId, int month, int year)
        {
            List<UserAccount> accounts = _context.Accounts.ToList();
            ViewBag.Accounts = accounts;

            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            int numDays = DateTime.DaysInMonth(year, month);
            List<double> data = new List<double>();

            for (var i = 1; i <= numDays; i++)
            {
                double valueIncomes = 0;
                var incomes = _context.Transaction.Where(o => o.Type && o.Account.ID == accountId && o.Date.Year == year && o.Date.Month == month && o.Date.Day == i);
                foreach (var income in incomes)
                {
                    valueIncomes += income.Value;
                }

                double valueExpenses = 0;
                var expenses = _context.Transaction.Where(o => !o.Type && o.Account.ID == accountId && o.Date.Year == year && o.Date.Month == month && o.Date.Day == i);
                foreach (var expense in expenses)
                {
                    valueExpenses += expense.Value;
                }
                
                data.Add(valueIncomes - valueExpenses);
            }
            return JsonConvert.SerializeObject(data);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
