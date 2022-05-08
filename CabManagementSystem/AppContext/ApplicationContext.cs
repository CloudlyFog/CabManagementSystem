using CabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class ApplicationContext : DbContext
    {
        public const string QUERYCONNECTION = "Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=CabManagementSystem;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

        public DbSet<UserModel> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
        /// changes user`s data
        /// needed properties: last & new Name, last & new Email, last & new Password
        /// changeableUser is an instance of UserModel which will be change to data of inputUser
        /// </summary>
        /// <param name="changeableUser"></param>
        /// <param name="inputUser"></param>
        public void EditUserData(UserModel user)
        {
            Users.Update(user);
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
        /// determines whether any user's data of sequence satisfies a condition
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns>returns <see langword="true"/> if user exists</returns>
        public bool IsExist(UserModel receivedUser) => Users.Any(user => user.Email == receivedUser.Email && user.Password == receivedUser.Password);

        /// <summary>
        /// determines whether any user's data of sequence satisfies a condition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns>returns <see langword="true"/> if user exist</returns>
        public bool IsExist(string name, string email) => Users.Any(user => user.Name == name && user.Email == email);

        /// <summary>
        /// determines whether any user's data of sequence satisfies a condition
        /// </summary>
        /// <param name="email"></param>
        /// <returns>returns <see langword="true"/> if user exists</returns>
        public bool IsExist(string email) => Users.Any(user => user.Email == email);

        /// <summary>
        /// gets user's password with definite ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetPassword(Guid ID) => Users.FirstOrDefault(user => user.ID == ID).Password;

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
        /// returns any existing property of user`s model
        /// can return: PostID, Name, Email, Password, Autenticated, Access
        /// properties NoteModel and MessageModel are not include in context and the method can`t return it
        /// needed properties: ID, for searching definite property
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userProp"></param>
        /// <returns></returns>
        public string GetUserProp(Guid ID, string userProp)
        {
            string name;
            string queryStringGetName = $"SELECT {userProp} FROM Users WHERE ID LIKE '{ID}'";
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
    }
}
