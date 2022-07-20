using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using CabManagementSystem.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using UserModel = CabManagementSystem.Models.UserModel;

namespace CabManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderRepository<OrderModel> orderRepository;
        private readonly IDriverRepository<DriverModel> driverRepository;
        private readonly IUserRepository<UserModel> userRepository;
        private const string queryConnectionBank = @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public ExceptionModel Exception { get; set; }
        public HomeController()
        {
            orderRepository = new OrderRepository(queryConnectionBank);
            driverRepository = new OrderRepository(queryConnectionBank);
            userRepository = new UserRepository();
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

        [Route("Privacy")]
        public IActionResult Privacy() => View();

        [Route("Error")]
        public IActionResult Error(UserModel user) => View(user);

        [HttpPost, Route("OrderTaxi")]
        public IActionResult OrderTaxi(UserModel user)
        {
            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            if (!userRepository.Exist(user.Order.UserID) || orderRepository.AlreadyOrder(user.Order.UserID))
                return RedirectToAction("Index", "Home");

            try
            {
                var operation = orderRepository.Create(user.Order);
                Exception = operation;
                if (operation != ExceptionModel.Successfull)
                {
                    user.Exception = operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception)
            {
                user.Exception = Exception;
                return Error(user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("EditOrder")]
        public IActionResult EditOrder(UserModel user)
        {
            if (!userRepository.Exist(user.ID) && orderRepository.AlreadyOrder(user.ID))
                return RedirectToAction("Index", "Home");

            user.Order.UserID = HttpContext.Session.GetString("userID") is not null
                    ? new(HttpContext.Session.GetString("userID")) : new();

            user.Order.ID = HttpContext.Session.GetString("orderID") is not null
                    ? new(HttpContext.Session.GetString("orderID")) : new();


            var order = orderRepository.Get(x => x.ID == user.Order.ID);
            order.Address = user.Order.Address;
            order.PhoneNumber = user.Order.PhoneNumber;
            order.Description = user.Order.Description;

            if (order is null)
                return RedirectToAction("Index", "Home");

            try
            {
                var operation = orderRepository.Update(order);
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
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("OrderCancellation")]
        public IActionResult OrderCancellation(UserModel user)
        {
            if (!userRepository.Exist(user.ID) && orderRepository.AlreadyOrder(user.ID))
                return RedirectToAction("Index", "Home");

            user.ID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            user.Order = orderRepository.Get(x => x.UserID == user.ID);

            if (user.Order is null)
                return RedirectToAction("Index", "Home");

            try
            {
                var operation = orderRepository.Delete(user.Order);
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
            return RedirectToAction("Index", "Home");
        }
    }
}