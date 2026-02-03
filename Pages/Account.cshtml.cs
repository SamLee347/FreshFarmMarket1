using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment1.Models;
using Assignment1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Assignment1.Pages
{
    [Authorize]
    public class AccountModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AccountModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }
    }
}
