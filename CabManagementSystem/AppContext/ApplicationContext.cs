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

        internal protected DbSet<UserModel> Users { get; set; }
        internal protected DbSet<AdminHandlingModel> AdminHandling { get; set; }
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(queryConnection);
        }

        /// <summary>
        /// deserialize taxi's data from json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>instance of model UserModel with filled data of model TaxiModel</returns>
        internal protected List<TaxiModel> DeserializeTaxiData(string path) => JsonConvert.DeserializeObject<JsonTaxiModel>(File.ReadAllText(path)).TaxiList;

        /// <summary>
        /// get list of definite TaxiModel's property
        /// </summary>
        /// <param name="nameNewsPart"></param>
        /// <returns>list of: ID, DriverID, TaxiNumber, TaxiClass, Price, SpecialName<returns>
        internal protected static List<object> GetTaxiPropList(string taxiProp)
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
