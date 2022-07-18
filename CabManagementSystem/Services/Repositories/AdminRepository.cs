using CabManagementSystem.AppContext;
using CabManagementSystem.Models;

namespace CabManagementSystem.Services.Repositories
{
    public class AdminRepository
    {
        private readonly ApplicationContext applicationContext;
        public AdminRepository()
        {
            applicationContext = new();
        }

        /// <summary>
        /// gives admin rights to definite user
        /// </summary>
        /// <param name="ID"></param>
        public ExceptionModel GiveAdminRights(Guid ID)
        {
            if (!applicationContext.Users.Any(x => x.ID == ID))
                return ExceptionModel.OperationFailed;
            applicationContext.Users.First(x => x.ID == ID).Access = true;
            applicationContext.SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes admin rights from definite user
        /// </summary>
        /// <param name="ID"></param>
        public ExceptionModel RemoveAdminRights(Guid ID)
        {
            if (!applicationContext.Users.Any(x => x.ID == ID))
                return ExceptionModel.OperationFailed;
            applicationContext.Users.First(x => x.ID == ID).Access = false;
            applicationContext.SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// changes property SelectMode of specified user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="mode"></param>
        public ExceptionModel ChangeSelectMode(Guid userID, SelectModeEnum mode)
        {
            var selectMode = applicationContext.AdminHandling.FirstOrDefault(x => x.UserID == userID);
            if (selectMode is null)
                return ExceptionModel.OperationFailed;
            selectMode.SelectMode = mode;
            applicationContext.SaveChanges();
            return ExceptionModel.Successfull;
        }
    }
}
