using UserManagement.API.Endpoints;

namespace UserManagement.API.Extensions
{
    public static class EndpointExtensions
    {
        public static WebApplication MapUserManagementAPIEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");

            var auth = api.MapGroup("/auth");
            auth.MapAuthEndpoints();

            var users = api.MapGroup("/users");
            users.MapUsersEndpoints().RequireAuthorization("RequireUser");

            return app;
        }

    }
}
