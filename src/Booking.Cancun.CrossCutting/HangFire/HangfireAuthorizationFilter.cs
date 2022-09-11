using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Net.Http.Headers;

namespace Booking.Cancun.Infraestructure.CrossCutting.HangFire;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private static readonly ILogger _logger = Log.Logger;
    public string User { get; set; }
    public string Pass { get; set; }

    private const string _AuthenticationScheme = "Basic";

    public HangfireAuthorizationFilter()
    {
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var header = httpContext.Request.Headers["Authorization"];

        if (MissingAuthorizationHeader(header))
            return SetChallengeResponse(httpContext);

        var authValues = AuthenticationHeaderValue.Parse(header);

        if (NotBasicAuthentication(authValues))
            return SetChallengeResponse(httpContext);

        var tokens = ExtractAuthenticationTokens(authValues);

        if (tokens.AreInvalid())
            return SetChallengeResponse(httpContext);

        if (tokens.CredentialsMatch(User, Pass))
            return true;

        _logger.Error(new Exception(),
            $"Invalid user: {tokens.Username} and password: {tokens.Password} authentication in hangfire dashboard.");

        return SetChallengeResponse(httpContext);
    }

    private static bool MissingAuthorizationHeader(StringValues header)
    {
        return string.IsNullOrWhiteSpace(header);
    }

    private static BasicAuthenticationToken ExtractAuthenticationTokens(AuthenticationHeaderValue authValues)
    {
        var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
        var parts = parameter.Split(':');
        return new BasicAuthenticationToken(parts);
    }

    private static bool NotBasicAuthentication(AuthenticationHeaderValue authValues)
    {
        return !_AuthenticationScheme.Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);
    }

    private bool SetChallengeResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
        return false;
    }
}
