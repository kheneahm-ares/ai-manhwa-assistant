using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Features.User;

namespace UserManagement.API.Endpoints
{
    public static class UsersEndpoint
    {
        public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
        {
            // map get user by email
            group.MapGet("{email}", GetUserByEmail);
            // map update display name
            group.MapPut("{email}", UpdateUser);

            return group;
        }

        private static async Task<IResult> GetUserByEmail([FromRoute] string email, 
                                                         [FromServices] UsersService userService)
        {
            var user = await userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                return Results.Ok(new
                {
                    user.Email,
                    user.DisplayName
                });
            }
            return Results.NotFound(new { Message = "User not found." });

        }

        // update user for now just updates display name
        private static async Task<IResult> UpdateUser([FromRoute] string email,
                                                            [FromQuery(Name = "displayName")] string newDisplayName,
                                                            [FromServices] UsersService userService)
        {
            var result = await userService.UpdateDisplayNameAsync(email, newDisplayName);
            if (result)
            {
                return Results.Ok(new { Message = "Display name updated successfully." });
            }
            return Results.NotFound(new { Message = "User not found." });
        }
    }
}
