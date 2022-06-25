using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly BankAccountContext bankAccountContext;
        public AccountController(ApplicationContext applicationContext, BankAccountContext bankAccountContext)
        {
            this.applicationContext = applicationContext;
            this.bankAccountContext = bankAccountContext;
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
            try
            {
                var operation = applicationContext.AddUser(user);
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
            return RedirectToAction("SignIn", "Account");
        }

        [HttpPost, Route("SelectBank")]
        public IActionResult SelectBank(UserModel user)
        {
            var userID = HttpContext.Session.GetString("userID") is not null
                    ? new Guid(HttpContext.Session.GetString("userID")) : new();

            if (!applicationContext.IsAuthanticated(userID))
                return RedirectToAction("Index", "Home");

            var bankAccountModel = bankAccountContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == userID);
            bankAccountModel.BankID = user.BankID;
            user = bankAccountContext.Users.FirstOrDefault(x => x.ID == userID);
            user.BankID = bankAccountModel.BankID;
            var operation = bankAccountContext.UpdateBankAccount(bankAccountModel, user);
            try
            {
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
