using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly TaxiContext taxiContext;
        private readonly ApplicationContext applicationContext;
        private readonly BankAccountContext bankAccountContext;
        private readonly BankContext bankContext;
        private readonly OrderContext orderContext;
        public AdminController(TaxiContext taxiContext, ApplicationContext applicationContext, BankAccountContext bankAccountContext, BankContext bankContext, OrderContext orderContext)
        {
            this.applicationContext = applicationContext;
            this.taxiContext = taxiContext;
            this.bankAccountContext = bankAccountContext;
            this.bankContext = bankContext;
            this.orderContext = orderContext;
        }

        public IActionResult Index(UserModel user)
        {
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            user.Order.UserID = user.ID;
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

            return View(user);
        }

        [HttpPost]
        public IActionResult AddTaxi(UserModel user)
        {
            if (!applicationContext.IsAuthanticated(user.ID) && !applicationContext.Users.FirstOrDefault(x => x.ID == user.ID).Access)
                return RedirectToAction("Index", "Admin");


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
            if (!applicationContext.IsAuthanticated(user.ID))
                return RedirectToAction("Index", "Admin");

            taxiContext.DeleteTaxi(user.Taxi);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult ChangeSelectMode(UserModel user, SelectModeEnum selectMode)
        {
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            if (!applicationContext.IsAuthanticated(user.ID))
                return RedirectToAction("Index", "Admin");

            applicationContext.ChangeSelectMode(user.ID, selectMode);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult GiveAdmin(Guid ID)
        {
            if (!applicationContext.IsAuthanticated(ID) && !applicationContext.Users.FirstOrDefault(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            applicationContext.GiveAdminRights(ID);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult RemoveAdmin(Guid ID)
        {
            if (!applicationContext.IsAuthanticated(ID) && !applicationContext.Users.FirstOrDefault(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            applicationContext.RemoveAdminRights(ID);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult Accrual(Guid ID, decimal BankAccountAmount)
        {
            var s = applicationContext.Users.FirstOrDefault(x => x.ID == ID).Access;
            if (!applicationContext.IsAuthanticated(ID) && !applicationContext.Users.FirstOrDefault(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            bankAccountContext.Accrual(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == ID), BankAccountAmount);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult Withdraw(Guid ID, decimal BankAccountAmount)
        {
            if (!applicationContext.IsAuthanticated(ID) && !applicationContext.Users.FirstOrDefault(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            bankAccountContext.Withdraw(bankContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == ID), BankAccountAmount);
            return RedirectToAction("Index", "Admin");
        }
    }
}
