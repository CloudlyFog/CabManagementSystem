using CabManagementSystem.Services.Interfaces;
using CabManagementSystem.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using BankAccountModel = BankSystem.Models.BankAccountModel;
using ExceptionModel = CabManagementSystem.Models.ExceptionModel;
using UserModel = CabManagementSystem.Models.UserModel;

namespace CabManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository<UserModel> userRepository;
        private readonly BankSystem.Services.Interfaces.IBankAccountRepository<BankAccountModel> bankAccountRepository;
        private const string queryConnectionBank = @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

        public AccountController()
        {
            userRepository = new UserRepository(queryConnectionBank);
            bankAccountRepository = new BankSystem.Services.Repositories.BankAccountRepository(queryConnectionBank);
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
            if (user is null)
                return RedirectToAction("Index", "Home");

            user.Password = userRepository.HashPassword(user.Password);
            var userID = userRepository.Get(x => x.Password == user.Password && x.Email == user.Email).ID;
            HttpContext.Session.SetString("userID", userID.ToString());

            if (userRepository.Exist(userID))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("SignIn", "Account");
        }

        [HttpPost, Route("SignUp")]
        public ActionResult SignUp(UserModel user)
        {
            if (user is null)
                return RedirectToAction("Index", "Home");

            user.ID = userRepository.Get(x => x.Password == user.Password && x.Email == user.Email).ID;
            HttpContext.Session.SetString("userID", user.ID.ToString());
            try
            {
                var operation = userRepository.Create(user);
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

            if (!userRepository.Exist(userID))
                return RedirectToAction("Index", "Home");

            try
            {
                var bankAccountModel = bankAccountRepository.Get(x => x.UserBankAccountID == userID);
                bankAccountModel.BankID = user.BankID;
                user = userRepository.Get(x => x.ID == userID);
                user.BankID = bankAccountModel.BankID;
                var operation = bankAccountRepository.Update(bankAccountModel, ConvertUserModel(user));
                if (operation != (BankSystem.Models.ExceptionModel)ExceptionModel.Successfull)
                {
                    user.Exception = (ExceptionModel)operation;
                    return RedirectToAction("Error", "Home", user);
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        private static BankSystem.Models.UserModel ConvertUserModel(UserModel user) => new()
        {
            ID = user.ID,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Authenticated = user.Authenticated,
            Access = user.Access,
            BankAccountID = user.BankAccountID,
            BankID = user.BankID,
            BankAccountAmount = user.BankAccountAmount
        };
    }
}
