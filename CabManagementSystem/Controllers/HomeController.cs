using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

            user = applicationContext.Users.FirstOrDefault(x => x.ID == user.ID) is not null
                ? applicationContext.Users.First(x => x.ID == user.ID) : new();

            var conditionForExistingRowOrder = orderContext.Orders.Any(x => x.UserID == user.ID);
            var conditionForExistingRowApplication = applicationContext.Users.Any(x => x.ID == user.ID);
            var conditionForExistingRowDriver = orderContext.Drivers.Any(x => x.Name == user.Order.DriverName);

            user.HasOrder = conditionForExistingRowApplication && applicationContext.Users.First(x => x.ID == user.ID).HasOrder;
            user.Access = conditionForExistingRowApplication && applicationContext.Users.First(x => x.ID == user.ID).Access;

            user.Order = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID) : new();
            user.Driver = orderContext.Drivers.Any(x => x.Name == user.Order.DriverName)
                ? orderContext.Drivers.First(x => x.Name == orderContext.Orders.First(x => x.UserID == user.ID).DriverName) : new();

            HttpContext.Session.SetString("orderID", user.Order.ID.ToString());
            HttpContext.Session.SetString("DriverName", user.Order.DriverName);

            return View(user);
        }

        [Route("Privacy")]
        public IActionResult Privacy() => View();

        [HttpPost, Route("OrderTaxi")]
        public IActionResult OrderTaxi(UserModel user)
        {
            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            if (!applicationContext.IsAuthanticated(user.Order.UserID) || orderContext.AlreadyOrder(user.Order.UserID))
                return RedirectToAction("Index", "Home");

            orderContext.CreateOrder(user.Order);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("EditOrder")]
        public IActionResult EditOrder(UserModel user)
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
        public IActionResult OrderCancellation(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID) && orderContext.AlreadyOrder(user.ID))
                return RedirectToAction("Index", "Home");

            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            user.Order = orderContext.Orders.FirstOrDefault(x => x.UserID == user.ID);
            orderContext.DeleteOrder(user.Order);
            return RedirectToAction("Index", "Home");
        }
    }
}