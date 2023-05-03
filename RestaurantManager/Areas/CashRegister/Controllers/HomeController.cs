using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManager.Areas.CashRegister.Controllers
{
    [Area("CashRegister")]
    [Authorize(Roles = "Admin, Cashier")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}