using DataAccess;
using Microsoft.EntityFrameworkCore;
using Core;
using Swashbuckle.AspNetCore.Swagger;
using Core.Interfaces;
using Llm;

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

            builder.Services.AddSingleton<ISentenceGenerator>(_ => new SentenceGenerator("http://127.0.0.1:11434", "hf.co/Vikhrmodels/Vistral-24B-Instruct-GGUF:Q4_K_M"));
            builder.Services.AddSingleton<ICardsQueue, CardQueue>();

            builder.Services.AddHostedService<SentenceGenerationWorker>();


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
