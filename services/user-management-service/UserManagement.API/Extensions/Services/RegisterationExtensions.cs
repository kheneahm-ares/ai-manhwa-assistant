using FluentValidation;
using UserManagement.API.Features.Auth;
using UserManagement.API.Features.User;

namespace UserManagement.API.Extensions.Services
{
    public static class RegisterationExtensions
    {
        // Add application services
        public static IServiceCollection AddRegisterationExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            // registrations
            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();
            services.AddScoped<IJWTGenerator, SymmetricJWTGenerator>();
            services.AddScoped<AuthService>();
            services.AddScoped<UsersService>();

            return services;
        }
    }
}
