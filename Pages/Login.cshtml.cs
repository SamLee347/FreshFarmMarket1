using Assignment1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }
        private readonly SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager { get; }
        private readonly ILogger<LoginModel> _logger;
        private readonly IAuditService _audit;
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, IAuditService audit)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _audit = audit;
        }
        public void OnGet() {}
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(LModel.EmailAddress, LModel.Password, isPersistent: LModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    await _audit.LogAsync("LoginSuccess");
                    return RedirectToPage("/Index");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too many failed attempts. Please try again later.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or password is incorrect");
                }
            }
            return Page();
        }
    }
}
