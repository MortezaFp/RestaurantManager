using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class AdjustmentsController : Controller
    {
        private readonly RestaurantManagerContext _db;

        public AdjustmentsController(RestaurantManagerContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _db.Adjustments.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Amount,IsMandatory,IsActive")] Adjustment adjustment)
        {
            if (ModelState.IsValid)
            {
                _db.Add(adjustment);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adjustment);
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null || _db.Adjustments == null)
            {
                return View("NotFound");
            }

            var adjustment = await _db.Adjustments.FindAsync(Id);
            if (adjustment == null)
            {
                return View("NotFound");
            }
            return View(adjustment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Amount,IsMandatory,IsActive")] Adjustment adjustment)
        {
            if (Id != adjustment.Id)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                _db.Update(adjustment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adjustment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var adjustment = await _db.Adjustments.FindAsync(Id);
            if (adjustment != null)
            {
                _db.Adjustments.Remove(adjustment);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}