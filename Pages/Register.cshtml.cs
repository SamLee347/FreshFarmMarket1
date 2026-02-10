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
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Assignment1.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment, IEmailSender emailSender)
        {
            _userManager = userManager;
            _environment = environment;
            _emailSender = emailSender;
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
                    var userId = await _userManager.GetUserIdAsync(user);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Page(
                        "/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId, token },
                    protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        RModel.EmailAddress,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

                    return RedirectToPage("RegisterConfirmation");
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
