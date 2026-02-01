using System.Security.Claims;

namespace ReadingProgress.API.Features
{
    public static class TestEndpoint
    {
        public static RouteGroupBuilder MapTestEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/test", TestAuth);
            return group;
        }


        private static async Task<IResult> TestAuth(ClaimsPrincipal user)
        {
            if (user.Identity?.IsAuthenticated == true)
            {
                return Results.Ok("Authenticated");
            }
            else
            {
                return Results.Unauthorized();
            }
        }
    }
}
