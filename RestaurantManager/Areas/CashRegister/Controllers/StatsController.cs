using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Data;
using RestaurantManager.Models;

namespace RestaurantManager.Areas.CashRegister.Controllers
{
    [Area("CashRegister")]
    [Authorize(Roles = "Admin, Cashier")]
    public class StatsController : Controller
    {
        private readonly RestaurantManagerContext _db;

        public StatsController(RestaurantManagerContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var viewModel = new RestaurantStatsViewModel();

            viewModel.TotalEarnings = _db.Orders.Sum(o => o.TotalPrice);

            viewModel.FoodEarnings = _db.FoodEarnings
                .FromSqlRaw("EXEC GetFoodEarnings")
                .ToList();

            viewModel.FoodQuantities = _db.FoodQuantities
                .FromSqlRaw("EXEC GetFoodQuantities")
                .ToList();

            viewModel.CategoryEarnings = _db.CategoryEarnings
                .FromSqlRaw("EXEC GetCategoryEarnings")
                .ToList();

            return View(viewModel);
        }
    }
}