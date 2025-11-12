using Microsoft.EntityFrameworkCore;
using FUNewsManagementSystem.Repo.Models;


namespace FUNewsManagementSystem.API.Configuration
{
    public static class DBConfiguration
    {
        public static IServiceCollection AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string 'DefaultConnection' is not configured.");
            }
            services.AddDbContext<NewsManagementDBContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
