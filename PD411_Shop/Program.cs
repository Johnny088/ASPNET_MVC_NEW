using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Data.Initializer;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("LocalDB")!;
    options.UseSqlServer(connectionString);
});

//builder.Services.AddDefaultIdentity<UserModel>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
//-----------------identity--------------------
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// add session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});


builder.Services.AddScoped<ProductRepository>();


// Add services
builder.Services.AddScoped<IEmailSender, EmailService>();

// add options

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


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

app.UseSession();


app.UseAuthentication(); //important!!! first Authentification then ==> Authorization
app.UseAuthorization();
app.MapRazorPages();

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

//Need to add name and surname to the register form
