using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestaurantManager.Data;
using RestaurantManager.Models;

/*using System.Data.Entity;*/

namespace RestaurantManager.Areas.Order.Controllers
{
    [Area("Order")]
    [Authorize(Roles = "Admin, Order Taker")]
    public class HomeController : Controller
    {
        private readonly RestaurantManagerContext _db;

        public HomeController(RestaurantManagerContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders.ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            CreateOrderViewModel orderViewModel = new();

            var foods = await _db.Foods.Where(f => f.IsActive && f.InStock).ToListAsync();
            var orders = await _db.Orders.ToListAsync();
            var adjustments = await _db.Adjustments.Where(a => a.IsActive).OrderBy(a => !a.IsMandatory).ToListAsync();

            foreach (var item in foods)
            {
                item.Category = await _db.FoodCategories.FindAsync(item.CategoryId);
            }

            orderViewModel.Foods = foods;
            orderViewModel.Adjustments = adjustments;

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string jsonInput)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(jsonInput);

            var model = data["model"];
            var orderItems = data["orderItems"];
            var adjustments = data["adjustments"];

            int SubTotalPrice = 0;
            foreach (Dictionary<string, string> orderItem in orderItems)
            {
                int price = int.Parse(orderItem["price"]);
                int quantity = int.Parse(orderItem["quantity"]);
                SubTotalPrice += price * quantity;
            }

            float TotalAdjustmentAmount = 0;
            foreach (Dictionary<string, string> adjustment in adjustments)
            {
                float amount = _db.Adjustments.Find(int.Parse(adjustment["id"])).Amount;
                TotalAdjustmentAmount += amount;
            }
            float TotalPrice = SubTotalPrice + SubTotalPrice * TotalAdjustmentAmount / 100;

            Models.Order order = new()
            {
                SubTotalPrice = SubTotalPrice,
                TotalPrice = TotalPrice,
                CustomerName = model[0]["value"],
                CustomerAddress = model[1]["value"],
                CustomerMobileNumber = model[2]["value"],
                IsTakeAway = bool.Parse(model[3]["value"]),
                CreatedAt = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                _db.Add(order);
                await _db.SaveChangesAsync();
                int orderId = order.Id;

                foreach (Dictionary<string, string> orderItem in orderItems)
                {
                    OrderItem newOrderItem = new()
                    {
                        OrderId = orderId,
                        FoodId = int.Parse(orderItem["id"]),
                        Quantity = int.Parse(orderItem["quantity"]),
                        Price = double.Parse(orderItem["price"])
                    };

                    _db.Add(newOrderItem);
                    await _db.SaveChangesAsync();
                }

                foreach (Dictionary<string, string> adjustment in adjustments)
                {
                    MapOrderAdjustment mapOrderAdjustment = new()
                    {
                        OrderId = orderId,
                        AdjustmentId = int.Parse(adjustment["id"])
                    };
                    _db.Add(mapOrderAdjustment);
                    await _db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var order = await _db.Orders.FindAsync(Id);
            if (order != null)
            {
                _db.Orders.Remove(order);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}