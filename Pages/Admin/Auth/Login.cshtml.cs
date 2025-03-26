using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using baitap.Models;

namespace baitap.Pages.Admin.Auth
{
    [Area("Admin")]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public LoginViewModel Input { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToPage("/Admin/Dashboard");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    
                    if (!isAdmin)
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "Bạn không có quyền truy cập vào trang quản trị");
                        return Page();
                    }
                    
                    return RedirectToPage("/Admin/Dashboard");
                }
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng");
            }
            return Page();
        }
    }
}