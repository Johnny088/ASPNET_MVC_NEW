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
