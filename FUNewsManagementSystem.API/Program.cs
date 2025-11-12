
using FUNewsManagementSystem.API.Configuration;
using FUNewsManagementSystem.Repo.DTO;
using Serilog;
using System.Text.Json.Serialization;

namespace FUNewsManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(config)
                            .CreateLogger();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:3000",
                            "https://localhost:3000"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Logging.AddSerilog();
            builder.Services.AddSingleton(Log.Logger);
            builder.Services.AddSingleton<Serilog.Extensions.Hosting.DiagnosticContext>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            builder.Services.AddDbConfiguration(config);
            builder.Services.AddServicesConfiguration(config);
            builder.Services.AddJwtAuthenticationService(config);
            builder.Services.AddSwaggerService();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            });

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors("Frontend");

            app.MapControllers();

            app.Run();
        }
    }
}
