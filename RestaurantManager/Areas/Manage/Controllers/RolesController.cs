using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Areas.Identity.Data;
using RestaurantManager.Areas.Manage.Models;

namespace RestaurantManager.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<RestaurantManagerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<RestaurantManagerUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var UserRoleViewModel = new List<UserRoleViewModel>();
            foreach (RestaurantManagerUser user in users)
            {
                var thisViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };
                UserRoleViewModel.Add(thisViewModel);
            }
            return View(UserRoleViewModel);
        }

        private async Task<List<string>> GetUserRoles(RestaurantManagerUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return View("NotFound");

            ViewBag.UserName = user.UserName;
            var model = new List<EditUserRoleViewModel>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                var UserRoleViewModel = new EditUserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    UserRoleViewModel.Selected = true;
                }
                else
                {
                    UserRoleViewModel.Selected = false;
                }
                model.Add(UserRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<EditUserRoleViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}