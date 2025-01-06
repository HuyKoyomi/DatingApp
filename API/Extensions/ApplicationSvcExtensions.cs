using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationSvcExtensions
    {
        public static IServiceCollection AddApplicationSvc(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            //  add service to cantainer
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}