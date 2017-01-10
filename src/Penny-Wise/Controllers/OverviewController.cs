using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

            //int accountId = int.Parse(Request.Form["overview-select-account"]);
            //var account = await _context.Accounts.SingleOrDefaultAsync(a => a.ID == accountId);

            //var items = await _context.Transaction.Include("Account").Include("Category").Where(m => m.Account == account).ToListAsync();

            //var items = await _context.Transaction.Include("Account").Include("Category").Where(m => m.Account == account).ToListAsync();
            //return View(items);

            var items = await _context.Transaction.Include("Account").Include("Category").ToListAsync();
            return View(items);
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
