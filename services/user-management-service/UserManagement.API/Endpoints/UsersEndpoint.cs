using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Services.User;

namespace UserManagement.API.Endpoints
{
    public static class UsersEndpoint
    {
        public static RouteGroupBuilder MapUsersEndpoints(this RouteGroupBuilder group)
        {
            // map get user by email
            group.MapGet("email", GetUserByEmail);

            return group;
        }

        private static async Task<IResult> GetUserByEmail([FromQuery(Name = "email")] string email, 
                                                         [FromServices] UserService userService)
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
    }
}
