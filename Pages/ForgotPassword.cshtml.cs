using Assignment1.Models;
using Assignment1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _db;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ForgotPassword FModel { get; set; }

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, AuthDbContext db, IEmailSender emailSender) 
        {
            _userManager = userManager;
            _db = db;
            _emailSender = emailSender;
        }
        
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Implement password reset logic here (e.g., send reset email)
                // Retrieve user by email, generate token, send email with reset link, etc.
                var user = _userManager.FindByEmailAsync(FModel.EmailAddress).Result;
                if (user != null)
                {
                    // Generate password reset token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var forgotPasswordLink = Url.Page(
                        "/ResetPassword",
                        pageHandler: null,
                        values: new { user.Id, token },
                    protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        FModel.EmailAddress,
                        "Password Reset",
                        $"Reset your password by <a href='{forgotPasswordLink}'>clicking here</a>");

                    return RedirectToPage("/ForgotPasswordValidate");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No user found with that email address.");
                    return Page();
                }

            }

            return Page();
        }
    }
}