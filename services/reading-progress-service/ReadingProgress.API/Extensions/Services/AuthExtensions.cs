using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ReadingProgress.API.Extensions.Services
{
    public static class AuthExtensions
    {
        // Add authentication and authorization services
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {

            // CORS
            services.AddCors(options =>
            {
                //allows any requests from localhost;3000 to get into app
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000")
                        .AllowCredentials();
                });
            });


            // jwt authentication
            var key = configuration["JWT:Key"];
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = false, // for development
                        ValidateAudience = false, // for development
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });

            // Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUser", policy =>
                    policy.RequireAuthenticatedUser());
            });

            return services;
        }
    }
}
