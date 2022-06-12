using CabManagementSystem.AppContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("ConnectionToDbCabManagementSystem");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<OrderContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<TaxiContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<BankAccountContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<BankContext>(options => options.UseSqlServer(connection));
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
