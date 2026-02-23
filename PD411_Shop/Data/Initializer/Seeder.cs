namespace PD411_Shop.Data.Initializer
{
    public class Seeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            //getting the class from the DI
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
    }
}
