using ReadingProgress.API.Features.ReadingLists;

namespace ReadingProgress.API.Extensions.Services
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddRegistrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ReadingListsService>();

            return services;
        }
    }
}
