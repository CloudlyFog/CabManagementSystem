using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly TaxiContext taxiContext;
        private readonly ApplicationContext applicationContext;
        public AdminController(TaxiContext taxiContext, ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
            this.taxiContext = taxiContext;
        }

        public IActionResult Index(UserModel user)
        {
            return View(user);
        }

        [HttpPost]
        public void AddTaxi(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID))
                return;

            taxiContext.Taxi.Add(user.Taxi);
        }

        [HttpPost]
        public void EditTaxi(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID))
                return;

            taxiContext.Taxi.Update(user.Taxi);
        }

        [HttpPost]
        public void DeleteTaxi(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID))
                return;

            taxiContext.Taxi.Remove(user.Taxi);
        }
    }
}
