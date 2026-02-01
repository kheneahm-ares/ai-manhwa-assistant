using ReadingProgress.API.Features;
using ReadingProgress.API.Features.ReadingLists;
using ReadingProgress.API.Features.ReadingProgressEvents;

namespace ReadingProgress.API.Extensions
{
    public static class EndpointExtensions
    {
        public static WebApplication MapReadingProgressAPIEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");

            var readingProgress = api.MapGroup("/reading-progress");

            readingProgress.MapTestEndpoint().RequireAuthorization("RequireUser");

            var readingLists = readingProgress.MapGroup("/reading-lists");
            readingLists.MapReadingListsEndpoints().RequireAuthorization("RequireUser");

            var readingProgressEvents = readingProgress.MapGroup("/reading-progress-events");
            readingProgressEvents.MapReadingProgressEventsEndpoints().RequireAuthorization("RequireUser");

            return app;
        }
    }
}
