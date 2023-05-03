using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

/*using System.Data.Entity;*/

namespace RestaurantManager.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class FoodCategoriesController : Controller
    {
        private readonly RestaurantManagerContext _db;

        public FoodCategoriesController(RestaurantManagerContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            var categories = await _db.FoodCategories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] FoodCategory category)
        {
            if (ModelState.IsValid)
            {
                _db.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null || _db.FoodCategories == null)
            {
                return View("NotFound");
            }

            var category = await _db.FoodCategories.FindAsync(Id);
            if (category == null)
            {
                return View("NotFound");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name")] FoodCategory category)
        {
            if (Id != category.Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var category = await _db.FoodCategories.FindAsync(Id);
            if (category != null)
            {
                if (!_db.Foods.Any(x => x.CategoryId == category.Id))
                {
                    _db.FoodCategories.Remove(category);
                }
                else
                {
                    TempData["ErrorMessage"] = "There are foods in this category.";
                    return RedirectToAction("Index");
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}