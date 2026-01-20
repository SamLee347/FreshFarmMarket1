using Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FullName = RModel.FullName,
                    UserName = RModel.EmailAddress,
                    Email = RModel.EmailAddress,
                    CreditCardNumber = RModel.CreditCardNumber,
                    Gender = RModel.Gender,
                    MobileNo = RModel.MobileNo,
                    DeliveryAddress = RModel.DeliveryAddress,
                    Photo = RModel.Photo.ToString(),
                    Description = RModel.Description
                };
                var result = await _userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
