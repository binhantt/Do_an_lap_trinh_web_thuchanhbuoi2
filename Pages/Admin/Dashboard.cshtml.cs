using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using baitap.Data;
using Microsoft.EntityFrameworkCore;

namespace baitap.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalOrders { get; set; }

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            TotalProducts = await _context.Products.CountAsync();
            TotalCategories = await _context.Categories.CountAsync();
            TotalOrders = 0; // Will implement later
        }

        public async Task<JsonResult> OnGetStatisticsAsync()
        {
            var stats = new
            {
                totalProducts = await _context.Products.CountAsync(),
                totalCategories = await _context.Categories.CountAsync(),
                latestProducts = await _context.Products
                    .OrderByDescending(p => p.Id)
                    .Take(5)
                    .Select(p => new { p.Name, p.Price })
                    .ToListAsync()
            };
            return new JsonResult(stats);
        }
    }
}