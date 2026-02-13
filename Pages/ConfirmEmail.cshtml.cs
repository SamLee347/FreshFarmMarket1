using Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private SignInManager<ApplicationUser> _signInManager { get; }
    private readonly IAuditService _audit;
    private readonly IPasswordHistoryService _passwordHistory;

    public ConfirmEmailModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IAuditService audit,
        IPasswordHistoryService passwordHistory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _audit = audit;
        _passwordHistory = passwordHistory;
    }

    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        StatusMessage = result.Succeeded
            ? "Thank you for confirming your email."
            : "Error confirming your email.";

        await _signInManager.SignInAsync(user, isPersistent: false);
        await _audit.LogAsync("Register", "Users", user.Id, null, user);
        await _passwordHistory.LogAsync();

        return RedirectToPage("/Index");
    }
}