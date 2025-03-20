using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using baitap.Data;
using baitap.Models;

namespace baitap.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get; set; } = default!;
        public IList<Product> Products { get; set; } = default!;
        public int? CategoryId { get; set; }

        public async Task OnGetAsync(int? categoryId)
        {
            CategoryId = categoryId;
            Categories = await _context.Categories.ToListAsync();
            
            var query = _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            Products = await query.ToListAsync();
        }
    }
}
