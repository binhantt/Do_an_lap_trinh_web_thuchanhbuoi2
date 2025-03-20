using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using baitap.Data;
using baitap.Models;

namespace baitap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId)
        {
            return RedirectToPage("/Index", new { categoryId });
        }

        public IActionResult Details(int id)
        {
            return RedirectToPage("/Products/Details", new { id });
        }
    }
}