
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebAPI.Authentication
{
    public class CustomAuthenticationHandler(
        IConfiguration configuration,
        IOptionsMonitor<CustomAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : AuthenticationHandler<CustomAuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly IConfiguration _configuration = configuration;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Token", out Microsoft.Extensions.Primitives.StringValues token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token not found in header."));
            }
            if (TokenIsValid(token))
            {
                ClaimsIdentity identity = new(Scheme.Name);
                ClaimsPrincipal principal = new(identity);
                AuthenticationTicket ticket = new(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Token is not valid."));
            }
        }
        private bool TokenIsValid(string? token)
        {
            return token?.Equals(_configuration["Settings:Token"]) ?? false;
        }
    }

}
