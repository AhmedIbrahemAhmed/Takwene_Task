using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MusicDistribution.BLL.Services;
using MusicDistribution.DAL;
using System.Text;

namespace MusicDistribution.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MusicDistributionDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories
            builder.Services.AddScoped<DAL.Repositories.IArtistRepository, DAL.Repositories.ArtistRepository>();
            builder.Services.AddScoped<DAL.Repositories.ITrackRepository, DAL.Repositories.TrackRepository>();
            builder.Services.AddScoped<DAL.Repositories.IDspRepository, DAL.Repositories.DspRepository>();
            builder.Services.AddScoped<DAL.Repositories.ITrackDistributionRepository, DAL.Repositories.TrackDistributionRepository>();

            // Services
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddScoped<IDistributionService, DistributionService>();

            // JWT Auth
            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured.");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            builder.Services.AddControllers();

            // OpenAPI + JWT security scheme for Swagger UI
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    // Instantiate the CONCRETE class, store as the interface type
                    var bearerScheme = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter your JWT token (no 'Bearer ' prefix needed)"
                    };

                    document.Components.SecuritySchemes["Bearer"] = bearerScheme;

                    return Task.CompletedTask;
                });
            });

            var app = builder.Build();

            // Apply migrations + seed
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MusicDistributionDbContext>();
                db.Database.Migrate();
                SeedData.InitializeAsync(db).GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "MusicDistribution API");
                });
            }

            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}