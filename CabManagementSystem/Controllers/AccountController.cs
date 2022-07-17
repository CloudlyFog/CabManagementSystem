using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using CabManagementSystem.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private readonly BankAccountContext bankAccountContext;
        private readonly IRepository<UserModel> repository;
        public AccountController(ApplicationContext applicationContext, BankAccountContext bankAccountContext)
        {
            this.applicationContext = applicationContext;
            this.bankAccountContext = bankAccountContext;
            repository = new UserRepository();
        }

        [Route("SignUp")]
        public ActionResult SignUp() => View();

        [Route("SignIn")]
        public ActionResult SignIn() => View();

        [Route("SignOut")]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, Route("SignIn")]
        public ActionResult SignIn(UserModel user, string[]? args = null)
        {
            user.Password = applicationContext.HashPassword(user.Password);
            var userID = repository.Get(x => x.Password == user.Password && x.Email == user.Email).ID;
            HttpContext.Session.SetString("userID", userID.ToString());

            if (applicationContext.IsAuthanticated(userID))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("SignIn", "Account");
        }

        [HttpPost, Route("SignUp")]
        public ActionResult SignUp(UserModel user)
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
        public ActionResult SelectBank(UserModel? user)
        {
            var userID = HttpContext.Session.GetString("userID") is not null
                    ? new Guid(HttpContext.Session.GetString("userID")) : new();

            if (!applicationContext.IsAuthanticated(userID))
                return RedirectToAction("Index", "Home");

            try
            {
                var bankAccountModel = bankAccountContext.BankAccounts.FirstOrDefault(x => x.UserBankAccountID == userID);
                bankAccountModel.BankID = user.BankID;
                user = bankAccountContext.Users.FirstOrDefault(x => x.ID == userID);
                user.BankID = bankAccountModel.BankID;
                var operation = bankAccountContext.UpdateBankAccount(bankAccountModel, user);
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
