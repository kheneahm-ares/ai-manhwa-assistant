using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.API.Features.User;
using UserManagement.API.Features.Users.DTOs;

namespace UserManagement.API.Endpoints
{
    public static class UsersEndpoint
    {
        public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
        {
            // map get user by email
            group.MapGet("", GetUserByEmail);
            // map update display name
            group.MapPut("", UpdateUser);

            return group;
        }

        private static async Task<IResult> GetUserByEmail([FromServices] UsersService userService,
                                                           ClaimsPrincipal userClaims)
        {
            var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return Results.Unauthorized();
            }

            var user = await userService.GetUserByEmailAsync(userEmail);

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
        private static async Task<IResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO,
                                                            [FromServices] UsersService userService,
                                                            ClaimsPrincipal userClaims)
        {
            var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return Results.Unauthorized();
            }

            var result = await userService.UpdateDisplayNameAsync(userEmail, updateUserDTO.DisplayName);
            if (result)
            {
                return Results.Ok(new { Message = "Display name updated successfully." });
            }
            return Results.NotFound(new { Message = "User not found." });
        }
    }
}
