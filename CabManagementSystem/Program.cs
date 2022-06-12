using CabManagementSystem.AppContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("ConnectionToDbCabManagementSystem");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection))
                .AddDbContext<OrderContext>(options => options.UseSqlServer(connection))
                .AddDbContext<TaxiContext>(options => options.UseSqlServer(connection))
                .AddDbContext<BankAccountContext>(options => options.UseSqlServer(connection))
                .AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

app.Run();
