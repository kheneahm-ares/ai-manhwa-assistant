using Microsoft.AspNetCore.Mvc;
using ReadingProgress.API.Features.ReadingLists.DTOs;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ReadingProgress.API.Features.ReadingLists
{
    public static class ReadingListsEndpoint
    {
        public static RouteGroupBuilder MapReadingListsEndpoints(this RouteGroupBuilder group)
        {
            // Add Reading List (POST)
            group.MapPost("/", AddReadingList);

            return group;
        }

        private static async Task<IResult> AddReadingList([FromServices] ReadingListsService service,
                                                           [FromBody] AddReadingListRequest request,
                                                           ClaimsPrincipal userClaims)
        {
            //get user id from sub/nameidentifier claim
            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var result = await service.AddReadingList(userId, request.ManhwaId);

            return result ? Results.Created() : Results.BadRequest();
        }


    }
}
