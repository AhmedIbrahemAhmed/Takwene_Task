
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicDistribution.BLL.Services;
using MusicDistribution.DAL;

namespace MusicDistribution.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Configure database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DAL.MusicDistributionDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register repositories
            builder.Services.AddScoped<DAL.Repositories.IArtistRepository, DAL.Repositories.ArtistRepository>();
            builder.Services.AddScoped<DAL.Repositories.ITrackRepository, DAL.Repositories.TrackRepository>();
            builder.Services.AddScoped<DAL.Repositories.IDspRepository, DAL.Repositories.DspRepository>();
            builder.Services.AddScoped<DAL.Repositories.ITrackDistributionRepository, DAL.Repositories.TrackDistributionRepository>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddScoped<IDistributionService, DistributionService>();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Seed data at startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MusicDistributionDbContext>();
                // Ensure database is created and seeded. Use synchronous wait because Main is not async.
                SeedData.InitializeAsync(db).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
