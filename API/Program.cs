using DataAccess;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Core;
using Swashbuckle.AspNetCore.Swagger;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
            builder.Services.AddScoped<Core.Interfaces.IDatabaseContext, DatabaseContext>();

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                DatabaseContext db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                DbInitializer.Initialize(db);
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}
