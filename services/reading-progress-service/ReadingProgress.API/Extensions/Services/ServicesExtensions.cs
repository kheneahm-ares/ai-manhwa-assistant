using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadingProgress.API.Features.ReadingLists;
using ReadingProgress.Data;
using System;

namespace ReadingProgress.API.Extensions.Services
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DB
            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ReadingProgress"));
            });


            services.AddAuthServices(configuration);
            services.AddRegistrationServices(configuration);

            return services;
        }
    }
}
