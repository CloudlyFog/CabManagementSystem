using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly OrderContext orderContext;
        private readonly TaxiContext taxiContext;
        private const string pathOrderTime = "D:/CabManagementSystem/CabManagementSystem/Data/Json/time.json";
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
            user.ID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70");

            var conditionForExistingRowOrder = orderContext.Orders.Any(x => x.UserID == user.ID);
            var conditionForExistingRowApp = applicationContext.Users.Any(x => x.ID == user.ID);

            user.HasOrder = conditionForExistingRowApp && applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).HasOrder;
            user.Access = conditionForExistingRowApp && applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).Access;

            user.Order.UserID = user.ID;
            user.Order = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID) : new();

            user.Driver.DriverID = taxiContext.Drivers.Any(x => x.Name == user.Order.DriverName)
                ? taxiContext.Drivers.FirstOrDefault(x => x.Name == user.Order.DriverName).DriverID : new();

            var conditionForExistingRowDriver = taxiContext.Drivers.Any(x => x.DriverID == user.Driver.DriverID);

            user.Taxi = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID) : new();


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

            if (!applicationContext.IsAuthanticated(user.ID))
                return RedirectToAction("Index", "Home");

            if (orderContext.AlreadyOrder(user.ID))
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