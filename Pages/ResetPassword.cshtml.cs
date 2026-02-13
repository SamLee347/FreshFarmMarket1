using Assignment1.Models;
using Assignment1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment1.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuditService _audit;
        private readonly IPasswordHistoryService _passwordHistory;
        private SignInManager<ApplicationUser> _signInManager { get; }
        private readonly AuthDbContext _db;
        [BindProperty]
        public string Token { get; set; }
        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public ResetPassword RPModel { get; set; }

        public ResetPasswordModel(
            UserManager<ApplicationUser> userManager,
            IAuditService audit,
            IPasswordHistoryService passwordHistory,
            SignInManager<ApplicationUser> signInManager,
            AuthDbContext db)
        {
            _userManager = userManager;
            _audit = audit;
            _passwordHistory = passwordHistory;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(string Id, string token)
        {
            if (Id == null || token == null)
            {
                return RedirectToPage("/Index");
            }

            UserId = Id;
            Token = token;

            // Check if valid user
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Id}'.");
            }

            // Check if valid token
            var isTokenValid = await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                token);
            if (!isTokenValid)
            {
                return BadRequest("Invalid password reset token.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user != null)
                {
                    var passwordHistory = _db.PasswordHistory
                        .Where(ph => ph.UserId == user.Id)
                        .OrderByDescending(ph => ph.ChangedAt)
                        .ToList();

                    if (passwordHistory != null)
                    {
                        // Check if the same password was used in the last 2 changes
                        foreach (var pastPassword in passwordHistory.Take(2))
                        {
                            // Compare the hash of the new password with the stored hash
                            var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, RPModel.NewPassword);
                            // Use PasswordHasher.VerifyHashedPassword to check if the new password matches the past hash
                            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, pastPassword.PasswordHash, RPModel.NewPassword);
                            if (result == PasswordVerificationResult.Success)
                            {
                                ModelState.AddModelError(string.Empty, "You cannot reuse a recently used password. Please choose a different password.");
                                return Page();
                            }
                        }
                    }

                    // Cannot change password again within 2 minutes of the previous change
                    var lastChange = passwordHistory.FirstOrDefault();
                    if (lastChange != null && (DateTime.UtcNow - lastChange.ChangedAt).TotalMinutes < 2)
                    {
                        ModelState.AddModelError(string.Empty, "You cannot change your password again within 2 minutes of the previous change. Please try again later.");
                        return Page();
                    }

                    var oldValues = user;
                    var changePasswordResult = await _userManager.ResetPasswordAsync(user, Token, RPModel.NewPassword);
                    if (changePasswordResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _audit.LogAsync(action: "ResetPassword", "Users", user.Id, oldValues, user);
                        await _passwordHistory.LogAsync();
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return Page();
        }
    }
}
