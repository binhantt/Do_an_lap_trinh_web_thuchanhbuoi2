using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using baitap.Data;
using baitap.Models;

namespace baitap.Pages.Admin.Products
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Product Product { get; set; } = new();
        
        [Required(ErrorMessage = "Vui lòng chọn hình ảnh")]
        [Display(Name = "Hình ảnh")]
        [BindProperty]
        public IFormFile? ImageFile { get; set; }
        
        public SelectList CategoryList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            CategoryList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CategoryList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
                return Page();
            }

            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    Product.ImageUrl = $"/images/products/{uniqueFileName}";
                }

                _context.Products.Add(Product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu sản phẩm. Vui lòng thử lại.");
                CategoryList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
                return Page();
            }
        }
    }
}