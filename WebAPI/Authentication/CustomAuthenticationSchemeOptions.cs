using Microsoft.AspNetCore.Authentication;

namespace WebAPI.Authentication
{
    public class CustomAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string Token { get; set; } = null!;
    }
}
