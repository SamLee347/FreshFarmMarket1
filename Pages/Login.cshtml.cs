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
        private readonly SignInManager<ApplicationUser> signInManager;
        public LoginModel(SignInManager<ApplicationUser> signInManager) {
            this.signInManager = signInManager;
        }
        public void OnGet() {}
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(LModel.EmailAddress, LModel.Password, LModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //Create the security context
                    //var claims = new List<Claim>
                    //{
                    //    new Claim(ClaimTypes.Name, LModel.EmailAddress),
                    //    new Claim("UserType","Default")

                    //};

                    //var i = new ClaimsIdentity(claims, "MyCookieAuth");
                    //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
                    //await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                    return RedirectToPage("/Index");
                }
                ModelState.AddModelError(string.Empty, "Username or password is incorrect");
            }
            return Page();
        }
    }
}
