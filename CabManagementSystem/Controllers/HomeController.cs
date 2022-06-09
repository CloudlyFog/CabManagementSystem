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
        private readonly TaxiContext taxiContext;
        public HomeController(ApplicationContext applicationContext, OrderContext orderContext, TaxiContext taxiContext)
        {
            this.applicationContext = applicationContext;
            this.orderContext = orderContext;
            this.taxiContext = taxiContext;
        }

        public IActionResult Index(UserModel user)
        {
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            //user.ID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70");

            user = applicationContext.Users.FirstOrDefault(x => x.ID == user.ID) is not null
                ? applicationContext.Users.First(x => x.ID == user.ID) : new();

            var conditionForExistingRowOrder = orderContext.Orders.Any(x => x.UserID == user.ID);
            var conditionForExistingRowApp = applicationContext.Users.Any(x => x.ID == user.ID);

            user.HasOrder = conditionForExistingRowApp && applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).HasOrder;
            user.Access = conditionForExistingRowApp && applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).Access;

            user.Order = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID) : new();

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
        public IActionResult OrderTaxi(UserModel user)
        {
            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            user.Order.UserID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70");

            if (!applicationContext.IsAuthanticated(user.Order.UserID))
                return RedirectToAction("Index", "Home");

            if (orderContext.AlreadyOrder(user.Order.UserID))
                return RedirectToAction("Index", "Home");
            //user.Order.Price = user.Taxi.Price;
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

        private static string ConvertAmount(string number)
        {
            if (number.Length == 3)
                return number;
            var sb = new StringBuilder();
            for (int i = 0; i < number.Length; i++)
            {
                if (i % 3 == 0)
                    sb.Append(' ');
                sb.Append(number[i]);
            }
            return sb.ToString();
        }
    }
}