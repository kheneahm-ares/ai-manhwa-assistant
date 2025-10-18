using FluentValidation;
using UserManagement.API.Services.Auth;
using UserManagement.API.Services.User;

namespace UserManagement.API.Extensions.Services
{
    public static class RegistationExtensions
    {
        // Add application services
        public static IServiceCollection AddRegistationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // registrations
            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
            services.AddScoped<IJWTGenerator, SymmetricJWTGenerator>();
            services.AddScoped<AuthService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
