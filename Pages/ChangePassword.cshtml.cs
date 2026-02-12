using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment1.Models;
using Assignment1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Assignment1.Pages
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuditService _audit;

        [BindProperty]
        public ChangePassword CPModel { get; set; }

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, IAuditService audit)
        {
            _userManager = userManager;
            _audit = audit;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var oldValues = user;
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, CPModel.Password, CPModel.NewPassword);
                    if (changePasswordResult.Succeeded)
                    {
                        await _audit.LogAsync(action: "PasswordChanged", "AspNetUsers", user.Id, oldValues, user);
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
