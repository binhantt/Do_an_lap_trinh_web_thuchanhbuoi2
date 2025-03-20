using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace baitap.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToPage("/Admin/Categories/Index");
        }

        public IActionResult Create()
        {
            return RedirectToPage("/Admin/Categories/Create");
        }

        public IActionResult Edit(int id)
        {
            return RedirectToPage("/Admin/Categories/Edit", new { id });
        }

        public IActionResult Delete(int id)
        {
            return RedirectToPage("/Admin/Categories/Delete", new { id });
        }
    }
}