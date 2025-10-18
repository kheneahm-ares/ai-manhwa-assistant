using UserManagement.API.Services.Auth;

namespace UserManagement.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("register", Register);
            group.MapPost("login", Login);

            return group;
        }

        private static async Task<IResult> Register(RegisterUserDTO user, AuthService authService)
        {
            var result = await authService.RegisterUser(user);
            if (result)
                return Results.Ok(new { Message = "User registered successfully." });
            return Results.BadRequest(new { Message = "User registration failed." });
        }

        private static async Task<IResult> Login(LoginUserDTO user, AuthService authService)
        {
            var result = await authService.LoginUser(user);

            if (!string.IsNullOrEmpty(result))
                return Results.Ok(new { accessToken = result });
            return Results.BadRequest(new { Message = "User login failed." });
        }
    }
}
