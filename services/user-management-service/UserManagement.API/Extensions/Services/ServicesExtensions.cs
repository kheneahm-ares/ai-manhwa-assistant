using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // transitive from npgsql
using System.Text;
using UserManagement.API.Extensions.Services;
using UserManagement.Data;

namespace UserManagement.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DB
            services.AddDbContext<AppDbContext>((options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("UserManagement"));
            });


            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthServices(configuration);
            services.AddRegistrationServices(configuration);

            return services;
        }
    }
}
