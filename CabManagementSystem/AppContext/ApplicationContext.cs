using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CabManagementSystem.AppContext
{
    public class ApplicationContext : DbContext
    {
        public const string QUERYCONNECTION = "Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

        public DbSet<UserModel> Users { get; set; }
        public DbSet<AdminHandlingModel> AdminHandling { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }


        /// <summary>
        /// adds user`s data in context
        /// needed properties: ID, Name, Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns><see langword="true"/> if user has been added correctly</returns>
        public void AddUser([Bind] UserModel receivedUser)
        {
            if (IsAuthanticated(receivedUser))//if user isn`t exist method will send false
                return;
            receivedUser.Authenticated = true;
            Users.Add(receivedUser);
            SaveChanges();
        }

        /// <summary>
        /// determines whether user is authanticated in the database
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns><see langword="true"/> if user was authenticate</returns>
        public bool IsAuthanticated(UserModel receivedUser) => Users.Any(user => user.Email == receivedUser.Email && user.Password == receivedUser.Password && user.Authenticated);

        /// <summary>
        /// determines whether user is authanticated in the database
        /// </summary>
        /// <param name="ID"></param>
        /// <returns><see langword="true"/> if user was authenticate</returns>
        public bool IsAuthanticated(Guid ID) => Users.Any(user => user.ID == ID && user.Authenticated);

        /// <summary>
        /// determines whether user's data satisfied a condition
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return ID of specified user</returns>
        public Guid GetID(UserModel user) => Users.Any(x => x.Name == GetUserProp(user, "Name") && x.Email == user.Email && x.Password == user.Password)
            ? Users.FirstOrDefault(x => x.Name == GetUserProp(user, "Name") && x.Email == user.Email && x.Password == user.Password).ID : new();

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
        public string GetUserProp(UserModel receivedUser, string userProp)
        {
            string name;
            string queryStringGetName = $"SELECT {userProp} FROM Users WHERE Email LIKE '{receivedUser.Email}' AND Password LIKE '{receivedUser.Password}'";
            SqlConnection connection = new(QUERYCONNECTION);
            SqlCommand command = new(queryStringGetName, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                name = reader.GetString(0);
            else
                name = "not found";
            reader.Close();
            connection.Close();
            return name;
        }

        /// <summary>
        /// serialize taxi's data in json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public void SerializeData(TaxiModel data, string path)
        {
            var jsonDes = JsonSerializer.Deserialize<JsonTaxiModel>(File.ReadAllText(path));
            if (jsonDes is null)
                return;
            jsonDes.TaxiList.Add(data);
            var json = JsonSerializer.Serialize(jsonDes);
            File.Delete(path);
            File.AppendAllText(path, json);
        }

        /// <summary>
        /// deserialize taxi's data from json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>instance of model UserModel with filled data of model TaxiModel</returns>
        public List<TaxiModel> DeserializeData(string path) => JsonSerializer.Deserialize<JsonTaxiModel>(File.ReadAllText(path)).TaxiList;

        /// <summary>
        /// get list of definite TaxiModel's property
        /// </summary>
        /// <param name="nameNewsPart"></param>
        /// <returns>list of: ID, DriverID, TaxiNumber, TaxiClass, Price, SpecialName<returns>
        public static List<object> GetTaxiPropList(string taxiProp)
        {
            var newsParts = new List<object>();
            var connection = new SqlConnection(QUERYCONNECTION);
            var command = new SqlCommand($"SELECT {taxiProp} FROM Taxi", connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
                newsParts.Add(reader.GetValue(0));
            reader.Close();
            connection.Close();
            return newsParts;
        }
    }
}
