using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using CabManagementSystem.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using BankAccountModel = BankSystem.Models.BankAccountModel;


namespace CabManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderRepository<OrderModel> orderRepository;
        private readonly IDriverRepository<DriverModel> driverRepository;
        private readonly IUserRepository<UserModel> userRepository;
        private readonly ITaxiRepository<TaxiModel> taxiRepository;
        private readonly BankSystem.Services.Interfaces.IBankAccountRepository<BankAccountModel> bankAccountRepository;
        private readonly AdminRepository adminRepository = new();
        private const string queryConnectionBank = @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public AdminController()
        {
            orderRepository = new OrderRepository(queryConnectionBank);
            driverRepository = new OrderRepository(queryConnectionBank);
            userRepository = new UserRepository();
            taxiRepository = new TaxiRepository();
            bankAccountRepository = new BankSystem.Services.Repositories.BankAccountRepository();
        }

        public IActionResult Index(UserModel user)
        {
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            user = userRepository.Get(user.ID);

            user.Order = orderRepository.Get(x => x.UserID == user.ID);
            user.Driver = driverRepository.Get(user.Order.Driver.DriverID);

            HttpContext.Session.SetString("orderID", user.Order.ID.ToString());
            HttpContext.Session.SetString("DriverName", user.Order.DriverName);

            return View(user);
        }

        [HttpPost]
        public IActionResult AddTaxi(UserModel user)
        {
            if (!userRepository.Exist(user.ID) && !userRepository.Get(user.ID).Access)
                return RedirectToAction("Index", "Admin");

            try
            {
                var operation = taxiRepository.Create(user.Taxi);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult EditTaxi(UserModel user)
        {
            if (!userRepository.Exist(user.ID))
                return RedirectToAction("Index", "Admin");

            user.Taxi.DriverID = taxiRepository.Get(x => x.ID == user.Taxi.ID).DriverID;
            taxiRepository.ChangeTracker();

            try
            {
                var operation = taxiRepository.Update(user.Taxi);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteTaxi(UserModel user)
        {
            if (!userRepository.Exist(user.ID))
                return RedirectToAction("Index", "Admin");
            try
            {
                var operation = taxiRepository.Delete(user.Taxi);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult ChangeSelectMode(UserModel user, SelectModeEnum selectMode)
        {
            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            if (!userRepository.Exist(user.ID))
                return RedirectToAction("Index", "Admin");

            try
            {
                var operation = adminRepository.ChangeSelectMode(user.ID, selectMode);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {

                return Content($"Error: {ex.Message}");
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult GiveAdmin(Guid ID)
        {
            if (!userRepository.Exist(ID) && !userRepository.Get(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            try
            {
                var operation = adminRepository.GiveAdminRights(ID);
                var user = userRepository.Get(x => x.ID == ID);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {

                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult RemoveAdmin(Guid ID)
        {
            if (!userRepository.Exist(ID) && !userRepository.Get(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");
            try
            {
                var operation = adminRepository.RemoveAdminRights(ID);
                var user = userRepository.Get(x => x.ID == ID);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {

                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult Accrual(Guid ID, decimal BankAccountAmount)
        {
            var user = userRepository.Get(x => x.ID == ID);
            if (!userRepository.Exist(ID) && !userRepository.Get(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            try
            {
                var operation = (ExceptionModel)bankAccountRepository.Accrual(bankAccountRepository.Get(x => x.UserBankAccountID == ID), BankAccountAmount);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult Withdraw(Guid ID, decimal BankAccountAmount)
        {
            var user = userRepository.Get(x => x.ID == ID);
            if (!userRepository.Exist(ID) && !userRepository.Get(x => x.ID == ID).Access)
                return RedirectToAction("Index", "Admin");

            try
            {
                var operation = (ExceptionModel)bankAccountRepository.Withdraw(bankAccountRepository.Get(x => x.UserBankAccountID == ID), BankAccountAmount);
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}
