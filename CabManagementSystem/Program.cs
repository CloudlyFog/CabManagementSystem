using CabManagementSystem.AppContext;

var builder = WebApplication.CreateBuilder(args);
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

app.MapDefaultControllerRoute();

app.Run();
