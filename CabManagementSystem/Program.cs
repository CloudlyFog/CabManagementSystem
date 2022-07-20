using CabManagementSystem.AppContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("ConnectionToDbCabManagementSystem");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>()
                .AddDbContext<OrderContext>()
                .AddDbContext<TaxiContext>()
                .AddSession();
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
