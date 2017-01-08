using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Penny_Wise.Data;
using Penny_Wise.Models;

namespace Penny_Wise.Controllers
{
    public class ProfileViewModel
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = "";
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // GET: /Profile/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var viewmodel = new ProfileViewModel
            {
                Name = user.Name,
                Email = user.Email
            };
            return View(viewmodel);
        }

        // POST: /Profile/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            user.Name = model.Name;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, model.Password);
            
            var result = await _userManager.UpdateAsync(user);

            foreach (var identityError in result.Errors)
                ModelState.AddModelError(identityError.Code, identityError.Description);

            if (!result.Succeeded)
                return View(model);

            return RedirectToAction("Index");
        }
        
        // GET: /Profile/DeleteAccount
        [HttpGet]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _userManager.DeleteAsync(user);
            
            return Redirect("~/");
        }

        public IActionResult Error()
        {
            return View();
        }
        
    }
}
