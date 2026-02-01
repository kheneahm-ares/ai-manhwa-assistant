using Microsoft.AspNetCore.Mvc;
using ReadingProgress.API.Features.ReadingProgressEvents.DTOs;
using System.Security.Claims;

namespace ReadingProgress.API.Features.ReadingProgressEvents
{
    public static class ReadingProgressEventsEndpoints
    {
        public static RouteGroupBuilder MapReadingProgressEventsEndpoints(this RouteGroupBuilder group)
        {
            // Add Reading Progress Event (POST)
            group.MapPost("/", AddReadingProgressEvent);
            return group;
        }

        private static async Task<IResult> AddReadingProgressEvent([FromServices] ReadingProgressEventsService service,
                                                                   [FromBody] AddReadingProgressEventRequest request,
                                                                   ClaimsPrincipal userClaims)
        {
            //get user id from sub/nameidentifier claim
            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var result = await service.AddReadingProgressEvent(userId, request.ManhwaId, request.ChapterNumber);
            return result ? Results.Created() : Results.BadRequest();
        }
    }
}
