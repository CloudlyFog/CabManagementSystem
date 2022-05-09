using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly OrderContext orderContext;
        public HomeController(ApplicationContext applicationContext, OrderContext orderContext)
        {
            this.applicationContext = applicationContext;
            this.orderContext = orderContext;
        }

        public IActionResult Index(UserModel user)
        {
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            user.Order.UserID = user.ID;
            bool predicateForExistingRow = orderContext.Orders.Any(x => x.UserID == user.ID);
            user.Order = new()
            {
                UserID = user.ID,
                PhoneNumber = predicateForExistingRow ? orderContext.Orders.First(x => x.UserID == user.ID).PhoneNumber : string.Empty,
                Description = predicateForExistingRow ? orderContext.Orders.First(x => x.UserID == user.ID).Description : string.Empty,
                Address = predicateForExistingRow ? orderContext.Orders.First(x => x.UserID == user.ID).Address : string.Empty,
                DriverName = predicateForExistingRow ? orderContext.Orders.First(x => x.UserID == user.ID).DriverName : string.Empty,
                ID = predicateForExistingRow ? orderContext.Orders.First(x => x.UserID == user.ID).ID : new()

            };
            HttpContext.Session.SetString("orderID", user.Order.ID.ToString());
            HttpContext.Session.SetString("DriverName", user.Order.DriverName);

            return View(user);
        }

        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost, Route("OrderTaxi")]
        public async Task<IActionResult> OrderTaxi(UserModel user)
        {
            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            if (!applicationContext.IsAuthanticated(user.ID) && orderContext.AlreadyOrder(user.ID))
                return RedirectToAction("Index", "Home");

            orderContext.CreateOrder(user.Order);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("EditOrder")]
        public async Task<IActionResult> EditOrder(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID) && orderContext.AlreadyOrder(user.ID))
                return RedirectToAction("Index", "Home");

            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                    ? new(HttpContext.Session.GetString("userID")) : new();
            user.Order.ID = HttpContext.Session.GetString("orderID") is not null
                    ? new(HttpContext.Session.GetString("orderID")) : new();
            orderContext.UpdateOrder(user.Order);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("OrderCancellation")]
        public async Task<IActionResult> OrderCancellation(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID) && orderContext.AlreadyOrder(user.ID))
                return RedirectToAction("Index", "Home");

            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            user.Order.ID = HttpContext.Session.GetString("orderID") is not null
                ? new(HttpContext.Session.GetString("orderID")) : new();
            user.Order.DriverName = HttpContext.Session.GetString("DriverName") is not null
                ? new(HttpContext.Session.GetString("DriverName")) : string.Empty;

            orderContext.DeleteOrder(user.Order);
            return RedirectToAction("Index", "Home");
        }
    }
}