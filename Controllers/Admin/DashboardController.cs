using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace baitap.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public DashboardController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Admin/Auth/Login");
            }

            if (!User.IsInRole("Admin"))
            {
                return RedirectToPage("/Admin/Auth/Login", new { area = "Admin" });
            }

            ViewBag.TotalProducts = 0;
            ViewBag.TotalCategories = 0;
            ViewBag.TotalOrders = 0;

            return RedirectToPage("/Admin/Dashboard");
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Admin/Auth/Login");
            }

            var stats = new
            {
                dailyVisits = 0,
                weeklyVisits = 0,
                monthlyVisits = 0
            };

            return Json(stats);
        }
    }
}