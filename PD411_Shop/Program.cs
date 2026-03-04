using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Data.Initializer;
using PD411_Shop.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("LocalDB")!;
    options.UseSqlServer(connectionString);
});


builder.Services.AddScoped<ProductRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
Seeder.Seed(app); // Seed the database with initial data
app.Run();



//need to check ==> dependency injection on the internet
// update? categories - CRUD

// multipart/form-data for sending the files, not string TYPE ==> IFormFile

//1.Якщо на головній сторінці к-сть товару 0 то зробити карточку темнішою як на розетці а також замість ціни вивести "Немає в наявності"

//2. Написати стоірнку із детальної інформацією про товар.
//	Відповідно на головній сторінці якщо натиснути на товар то повинно перенаправити користувача на стоірнку із детальної інформацією
