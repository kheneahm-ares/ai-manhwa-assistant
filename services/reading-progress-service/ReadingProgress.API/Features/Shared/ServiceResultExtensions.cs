using System.Runtime.CompilerServices;

namespace ReadingProgress.API.Features.Shared
{
    public static class ServiceResultExtensions
    {
        public static IResult ToHttp(this ServiceResult result)
        {
            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return result.ErrorType switch
            {
                ErrorType.NotFound => Results.NotFound(result.ErrorMessage),
                ErrorType.BadRequest => Results.BadRequest(result.ErrorMessage),
                ErrorType.Conflict => Results.Conflict(result.ErrorMessage),
                ErrorType.Unauthorized => Results.Unauthorized(),
                _ => Results.StatusCode(500)
            };
        }
    }
}
