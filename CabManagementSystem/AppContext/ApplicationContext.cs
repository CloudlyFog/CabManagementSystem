using CabManagementSystem.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CabManagementSystem.AppContext
{
    public class ApplicationContext : DbContext
    {
        public const string queryConnection = "Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

        public DbSet<UserModel> Users { get; set; }
        public DbSet<AdminHandlingModel> AdminHandling { get; set; }
        private readonly BankAccountContext bankAccountContext = new(new DbContextOptions<BankAccountContext>());
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(queryConnection);
        }

        /// <summary>
        /// adds user`s data in context
        /// needed properties: ID, Name, Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns>instance of an <see cref="ExceptionModel"/></returns>
        internal protected ExceptionModel AddUser(UserModel receivedUser)
        {
            if (IsAuthanticated(receivedUser))//if user isn`t exist method will send false
                return ExceptionModel.OperationFailed;
            receivedUser.Password = HashPassword(receivedUser.Password);
            receivedUser.Authenticated = true;
            receivedUser.ID = Guid.NewGuid();
            Users.Add(receivedUser);
            SaveChanges();
            var bankAccountModel = new BankAccountModel()
            {
                ID = receivedUser.BankAccountID,
                UserBankAccountID = receivedUser.ID
            };
            return bankAccountContext.AddBankAccount(bankAccountModel);
        }

        /// <summary>
        /// determines whether user is authanticated in the database
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns><see langword="true"/> if user was authenticate</returns>
        internal protected bool IsAuthanticated(UserModel receivedUser) => Users.Any(user => user.Email == receivedUser.Email && user.Password == receivedUser.Password && user.Authenticated);

        /// <summary>
        /// determines whether user is authanticated in the database
        /// </summary>
        /// <param name="ID"></param>
        /// <returns><see langword="true"/> if user was authenticate</returns>
        public bool IsAuthanticated(Guid ID) => Users.Any(user => user.ID == ID && user.Authenticated);

        /// <summary>
        /// gives admin rights to definite user
        /// </summary>
        /// <param name="ID"></param>
        public ExceptionModel GiveAdminRights(Guid ID)
        {
            if (!Users.Any(x => x.ID == ID))
                return ExceptionModel.OperationFailed;
            Users.First(x => x.ID == ID).Access = true;
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes admin rights from definite user
        /// </summary>
        /// <param name="ID"></param>
        public ExceptionModel RemoveAdminRights(Guid ID)
        {
            if (!Users.Any(x => x.ID == ID))
                return ExceptionModel.OperationFailed;
            Users.First(x => x.ID == ID).Access = false;
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// determines whether user's data satisfied a condition
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return ID of specified user</returns>
        internal protected Guid GetID(UserModel user) => Users.Any(x => x.Name == GetUserProperty(user.ID, "Name") && x.Email == user.Email && x.Password == user.Password)
            ? Users.FirstOrDefault(x => x.Name == GetUserProperty(user.ID, "Name") && x.Email == user.Email && x.Password == user.Password).ID : new();

        /// <summary>
        /// determines whether user's data satisfied a condition
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns>return ID of specified user</returns>
        internal protected Guid GetID(string password, string email) => Users.Any(x => x.Email == email && x.Password == password)
            ? Users.FirstOrDefault(x => x.Email == email && x.Password == password).ID : new();

        /// <summary>
        /// changes property SelectMode of specified user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="mode"></param>
        public ExceptionModel ChangeSelectMode(Guid userID, SelectModeEnum mode)
        {
            var selectMode = AdminHandling.FirstOrDefault(x => x.UserID == userID);
            if (selectMode is null)
                return ExceptionModel.OperationFailed;
            selectMode.SelectMode = mode;
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// can return: ID, Name, Email, Password, Autenticated, Access
        /// properties NoteModel and MessageModel are not include in context and the method can`t return it
        /// needed properties: Email and Password for searching definite property
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <param name="userProp"></param>
        /// <returns>
        /// returns any existing property of user`s model.
        /// can return: ID, Name, Email, Password, Autenticated, Access
        /// </returns>
        private string GetUserProperty(Guid userID, string userProp)
        {
            string name;
            var queryStringGetName = $"SELECT {userProp} FROM Users WHERE ID = '{userID}'";
            var connection = new SqlConnection(queryConnection);
            var command = new SqlCommand(queryStringGetName, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
                name = reader.GetString(0);
            else
                name = "not found";
            reader.Close();
            connection.Close();
            return name;
        }

        /// <summary>
        /// deserialize taxi's data from json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>instance of model UserModel with filled data of model TaxiModel</returns>
        public List<TaxiModel> DeserializeTaxiData(string path) => JsonConvert.DeserializeObject<JsonTaxiModel>(File.ReadAllText(path)).TaxiList;

        /// <summary>
        /// get list of definite TaxiModel's property
        /// </summary>
        /// <param name="nameNewsPart"></param>
        /// <returns>list of: ID, DriverID, TaxiNumber, TaxiClass, Price, SpecialName<returns>
        public static List<object> GetTaxiPropList(string taxiProp)
        {
            var newsParts = new List<object>();
            var connection = new SqlConnection(queryConnection);
            var command = new SqlCommand($"SELECT {taxiProp} FROM Taxi", connection);
            connection.Open();
            command.ExecuteNonQuery();
            var reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
                newsParts.Add(reader.GetValue(0));
            reader.Close();
            connection.Close();
            return newsParts;
        }

        /// <summary>
        /// Hashing string with SHA256 algorithm
        /// </summary>
        /// <param name="password"></param>
        /// <returns>hashed string</returns>
        internal protected string HashPassword(string password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
