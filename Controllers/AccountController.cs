using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace baitap.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl = null)
        {
            return RedirectToPage("/Account/Login", new { returnUrl });
        }

        public IActionResult Register(string returnUrl = null)
        {
            return RedirectToPage("/Account/Register", new { returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}