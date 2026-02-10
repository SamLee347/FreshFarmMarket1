using Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Web;

namespace Assignment1.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private SignInManager<ApplicationUser> _signInManager { get; }
        private IWebHostEnvironment _environment;

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }
        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FullName = RModel.FullName,
                    UserName = RModel.EmailAddress,
                    Email = RModel.EmailAddress,
                    CreditCardNumber = protector.Protect(RModel.CreditCardNumber),
                    Gender = RModel.Gender,
                    MobileNo = RModel.MobileNo,
                    DeliveryAddress = protector.Protect(RModel.DeliveryAddress),
                    Description = RModel.Description,
                };

                if (RModel.Photo != null)
                {
                    user.Photo = RModel.Photo.FileName;
                    var file = Path.Combine(_environment.WebRootPath, "Uploads", RModel.Photo.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await RModel.Photo.CopyToAsync(fileStream);
                    }
                }

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
