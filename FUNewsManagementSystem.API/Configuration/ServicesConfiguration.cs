using FUNewsManagementSystem.Repo.Mapper;
using FUNewsManagementSystem.Repo.Repositories;
using FUNewsManagementSystem_Service.Services;

namespace FUNewsManagementSystem.API.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //Add repository classes here
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            //Add service classes here
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();
            services.AddScoped<ITagService, TagService>();

            //Add custom services here
            services.AddScoped<IMapperlyMapper, MapperlyMapper>();

            return services;
        }
    }
}
