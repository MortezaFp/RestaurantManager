using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

/*using System.Data.Entity;*/

namespace RestaurantManager.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class FoodsController : Controller
    {
        private readonly RestaurantManagerContext _db;

        public FoodsController(RestaurantManagerContext context)
        {
            _db = context;
        }

        private async Task<SelectList> GetFoodCategories()
        {
            var categories = await _db.FoodCategories.ToListAsync();
            var list = categories.Select(i => new { i.Id, i.Name }).ToList();
            var categoryList = new SelectList(list, "Id", "Name");
            return categoryList;
        }

        private static async Task<string> SavePictureAndGetPath(IFormFile Picture)
        {
            var extension = Path.GetExtension(Picture.FileName);
            Guid id = Guid.NewGuid();
            var newFileName = $"{id}{extension}";
            var imagePath = $"/content/Images/Foods/{newFileName}";

            using (var fileStream = new FileStream($"wwwroot{imagePath}", FileMode.Create))
            {
                await Picture.CopyToAsync(fileStream);
            }
            return imagePath;
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _db.Foods.ToListAsync();

            foreach (var item in foods)
            {
                item.Category = await _db.FoodCategories.FindAsync(item.CategoryId);
            }

            return View(foods);
        }

        public async Task<IActionResult> Create()
        {
            var categoryList = await GetFoodCategories();
            ViewBag.FoodCategories = categoryList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Price,IsActive,InStock,PictureFile")] Food food)
        {
            if (ModelState.IsValid)
            {
                var imagePath = await SavePictureAndGetPath(food.PictureFile);
                food.PicturePath = imagePath;

                _db.Add(food);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(food);
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null || _db.Foods == null)
            {
                return NotFound();
            }

            var food = await _db.Foods.FindAsync(Id);
            if (food == null)
            {
                return NotFound();
            }

            var stream = System.IO.File.OpenRead("../RestaurantManager/wwwroot" + food.PicturePath);
            var File = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
            food.PictureFile = File;
            var categoryList = await GetFoodCategories();
            ViewBag.FoodCategories = categoryList;
            return View(food);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,CategoryId,Name,Price,IsActive,InStock,PictureFile,PicturePath")] Food food)
        {
            if (Id != food.Id)
            {
                return NotFound();
            }

            ModelState.Remove("PictureFile");

            if (ModelState.IsValid)
            {
                if (food.PictureFile != null)
                {
                    var imagePath = await SavePictureAndGetPath(food.PictureFile);
                    food.PicturePath = imagePath;
                }

                _db.Update(food);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var food = await _db.Foods.FindAsync(Id);
            if (food != null)
            {
                _db.Foods.Remove(food);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}