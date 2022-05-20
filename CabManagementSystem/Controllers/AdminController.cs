using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly TaxiContext taxiContext;
        private readonly ApplicationContext applicationContext;
        private const string PathSerialization = "D:/CabManagementSystem/CabManagementSystem/Data/Json/taxi.json";
        public AdminController(TaxiContext taxiContext, ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
            this.taxiContext = taxiContext;
        }

        public IActionResult Index(UserModel user)
        {
            //user.Taxi = applicationContext.DeserializeData<TaxiModel>(PathSerialization);
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            user.Order.UserID = user.ID;

            return View(user);
        }

        [HttpPost]
        public IActionResult AddTaxi(UserModel user)
        {
            user.ID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70");
            if (!applicationContext.IsAuthanticated(user.ID))
                return RedirectToAction("Index", "Admin");

            applicationContext.SerializeData<TaxiModel>(user.Taxi.TaxiList, PathSerialization);

            taxiContext.AddTaxi(user.Taxi);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult EditTaxi(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID))
                return RedirectToAction("Index", "Admin");

            user.Taxi.DriverID = taxiContext.Taxi.Any(x => x.ID == user.Taxi.ID)
                ? taxiContext.Taxi.FirstOrDefault(x => x.ID == user.Taxi.ID).DriverID : new();
            taxiContext.ChangeTracker.Clear();

            taxiContext.UpdateTaxi(user.Taxi);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteTaxi(UserModel user)
        {
            user.ID = new("A08AB3E5-E3EC-47CD-84EF-C0EB75045A70");
            if (!applicationContext.IsAuthanticated(user.ID))
                return RedirectToAction("Index", "Admin");

            taxiContext.DeleteTaxi(user.Taxi);
            return RedirectToAction("Index", "Admin");
        }
    }
}
