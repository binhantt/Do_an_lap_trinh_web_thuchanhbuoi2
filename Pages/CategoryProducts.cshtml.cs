using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using baitap.Data;
using baitap.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace baitap.Pages
{
    public class CategoryProductsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category Category { get; set; }
        public IList<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FindAsync(categoryId);

            if (Category == null)
            {
                return NotFound();
            }

            Products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            return Page();
        }
    }
}