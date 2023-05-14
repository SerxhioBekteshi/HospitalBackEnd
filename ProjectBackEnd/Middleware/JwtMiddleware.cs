using ProjectBackEnd.Utility;
using Service.Contracts;

namespace ProjectBackEnd.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils, IServiceManager serviceManager)
    {
        var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var validateTokenResult = jwtUtils.ValidateToken(accessToken);

        if (validateTokenResult.Item1.HasValue &&
            !string.IsNullOrWhiteSpace(validateTokenResult.Item2) &&
            !string.IsNullOrWhiteSpace(validateTokenResult.Item3))
        {
            var currentUser = await serviceManager.UserService.GetRecordById(validateTokenResult.Item1.Value);

            if (currentUser.TokenHash != validateTokenResult.Item3)
                context.Items["User"] = null;
            else
                context.Items["User"] = context.User;
        }
        await _next(context);
    }
}