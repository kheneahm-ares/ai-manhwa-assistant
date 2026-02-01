using FluentValidation;
using ReadingProgress.API.Features.ReadingLists;
using ReadingProgress.API.Features.ReadingProgressEvents;
using ReadingProgress.API.Features.ReadingProgressEvents.DTOs;

namespace ReadingProgress.API.Extensions.Services
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddRegistrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ReadingListsService>();
            services.AddScoped<ReadingProgressEventsService>();

            // fluent validation
            services.AddScoped<IValidator<AddReadingProgressEventRequest>, AddReadingProgressEventValidator>();

            return services;
        }
    }
}
