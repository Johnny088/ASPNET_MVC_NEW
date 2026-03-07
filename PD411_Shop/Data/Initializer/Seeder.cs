using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Models;
using System.Text.Json;
namespace PD411_Shop.Data.Initializer
{
    public class Seeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            //getting the AppDbContext from DI
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            



            context.Database.Migrate(); // handle all migrations like the command "update-database"
                                        //var products = new List<ProductModel>
                                        //{
                                        //    new ProductModel{Name = "COre1"},
                                        //    new ProductModel{Name = "Core2"},
                                        //};
                                        //var category = new CategoryModel
                                        //{
                                        //    Name = "Core",
                                        //    Products = products
                                        //};

            //context.Categories.Add(category);
            //context.SaveChanges();
            //Console.WriteLine(context.Categories.Count());

            //----------------- Users and Roles -------------------------
            if (!roleManager.Roles.Any())
            {
                var adminRole = new IdentityRole { Name = "admin" };
                var userRole = new IdentityRole { Name = "user" };
                roleManager.CreateAsync(adminRole).Wait();
                roleManager.CreateAsync(userRole).Wait();
                var admin = new UserModel
                {
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                    FirstName = "Neo",
                    LastName = "Wilson"
                };
                var user = new UserModel
                {
                    Email = "user@gmail.com",
                    UserName = "user",
                    EmailConfirmed = true,
                    FirstName = "testName",
                    LastName = "testSurname"
                };
                userManager.CreateAsync(admin, "qwerty").Wait(); //qwerty is a password
                userManager.CreateAsync(user, "qwerty").Wait();
                userManager.AddToRoleAsync(admin, "admin").Wait();
                userManager.AddToRoleAsync(user, "user").Wait();
            }

            if (!context.Categories.Any())
            {
                string root = Directory.GetCurrentDirectory();
                string path = Path.Combine(root, "wwwroot", "seed_data", "components.json");
                string json = File.ReadAllText(path);
                List<CategoryModel>? categories = JsonSerializer.Deserialize<List<CategoryModel>>(json);

                if(categories == null)
                {
                    return;
                }
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}
