using CabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystem.AppContext
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options) => Database.EnsureCreated();

        public DbSet<BankModel> Banks { get; set; }
        public DbSet<OperationModel> Operations { get; set; }


    }
}
