#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantManager.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<RestaurantManagerUser> _userManager;
        private readonly SignInManager<RestaurantManagerUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(
            UserManager<RestaurantManagerUser> userManager,
            SignInManager<RestaurantManagerUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(RestaurantManagerUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                await _userManager.ChangeEmailAsync(user, Input.NewEmail, code);
                await _userManager.SetUserNameAsync(user, Input.NewEmail);
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Email Changed.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

    }
}