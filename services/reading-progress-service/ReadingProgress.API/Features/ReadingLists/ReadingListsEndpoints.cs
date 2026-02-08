using Microsoft.AspNetCore.Mvc;
using ReadingProgress.API.Features.ReadingLists.DTOs;
using ReadingProgress.API.Features.Shared;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ReadingProgress.API.Features.ReadingLists
{
    public static class ReadingListsEndpoints
    {
        public static RouteGroupBuilder MapReadingListsEndpoints(this RouteGroupBuilder group)
        {
            // Add Reading List (POST)
            group.MapPost("/", AddReadingList);

            // Update Reading List (PUT)
            group.MapPut("/", UpdateReadingList);

            // Get Reading List (GET)
            group.MapGet("/", GetReadingList);

            return group;
        }

        private static async Task<IResult> GetReadingList([FromServices] ReadingListsService service, ClaimsPrincipal userClaims)
        {
            //get user id from sub/nameidentifier claim
            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }
            var result = await service.GetReadingList(userId);
            return result.ToHttp();
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

            return result.ToHttp();
        }

        private static async Task<IResult> UpdateReadingList([FromServices] ReadingListsService service,
                                                           [FromBody] UpdateReadingListRequest request,
                                                           ClaimsPrincipal userClaims)
        {
            //get user id from sub/nameidentifier claim
            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }
            var result = await service.UpdateReadingList(userId, request.ManhwaId, request.Status);
            return result.ToHttp();
        }

    }
}
