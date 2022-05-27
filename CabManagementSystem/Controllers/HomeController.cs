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

            bool conditionForExistingRowOrder = orderContext.Orders.Any(x => x.UserID == user.ID);
            bool conditionForExistingRowApp = applicationContext.Users.Any(x => x.ID == user.ID);

            user.HasOrder = conditionForExistingRowApp && applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).HasOrder;
            user.Access = conditionForExistingRowApp && applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).Access;
            user.Order.UserID = user.ID;

            user.Order = new()
            {
                UserID = user.ID,
                PhoneNumber = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID).PhoneNumber : string.Empty,
                Description = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID).Description : string.Empty,
                Address = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID).Address : string.Empty,
                DriverName = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID).DriverName : string.Empty,
                ID = conditionForExistingRowOrder ? orderContext.Orders.First(x => x.UserID == user.ID).ID : new()

            };

            user.Driver.DriverID = taxiContext.Drivers.Any(x => x.Name == user.Order.DriverName)
                ? taxiContext.Drivers.FirstOrDefault(x => x.Name == user.Order.DriverName).DriverID : new();

            bool conditionForExistingRowDriver = taxiContext.Drivers.Any(x => x.DriverID == user.Driver.DriverID);

            user.Taxi = new()
            {
                ID = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID).ID : new(),
                DriverID = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID).DriverID : new(),
                TaxiNumber = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID).TaxiNumber : string.Empty,
                TaxiClass = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID).TaxiClass : TaxiClass.Economy,
                Price = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID).Price : TaxiPrice.Economy,
                SpecialName = conditionForExistingRowDriver ? taxiContext.Taxi.First(x => x.DriverID == user.Driver.DriverID).SpecialName : string.Empty,
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

        private Guid GetDriverID()
        {
            return new();
        }
    }
}