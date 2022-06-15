using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext applicationContext;
        public AccountController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [Route("SignUp")]
        public IActionResult SignUp() => View();

        [Route("SignIn")]
        public IActionResult SignIn() => View();

        [Route("SignOut")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("SignIn")]
        public async Task<IActionResult> SignIn(UserModel user, string[]? args = null)
        {
            user.Password = applicationContext.HashPassword(user.Password);
            var userID = applicationContext.GetID(user);
            HttpContext.Session.SetString("userID", userID.ToString());

            if (applicationContext.IsAuthanticated(userID))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("SignIn", "Account");
        }

        [HttpPost, Route("SignUp")]
        public async Task<IActionResult> SignUp(UserModel user)
        {
            user.ID = applicationContext.GetID(user);
            HttpContext.Session.SetString("userID", user.ID.ToString());

            if (!applicationContext.IsAuthanticated(user.ID))
            {
                applicationContext.AddUser(user);
                return RedirectToAction("SignIn", "Account");
            }

            else
                return RedirectToAction("SignIn", "Account");
        }
    }
}
