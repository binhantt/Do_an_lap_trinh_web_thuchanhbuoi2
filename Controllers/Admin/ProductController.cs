using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace baitap.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToPage("/Admin/Products/Index");
        }

        public IActionResult Create()
        {
            return RedirectToPage("/Admin/Products/Create");
        }

        public IActionResult Edit(int id)
        {
            return RedirectToPage("/Admin/Products/Edit", new { id });
        }

        public IActionResult Delete(int id)
        {
            return RedirectToPage("/Admin/Products/Delete", new { id });
        }
    }
}