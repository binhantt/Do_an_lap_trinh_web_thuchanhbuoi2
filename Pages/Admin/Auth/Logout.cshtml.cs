using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace baitap.Pages.Admin.Auth
{
    [Area("Admin")]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Simple direct logout
            await _signInManager.SignOutAsync();
            
            // Force redirect to login page
            return RedirectToPage("/Admin/Auth/Login");
        }
    }
}