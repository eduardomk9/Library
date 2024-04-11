using Core.Business;
using Core.Dtos.Auth;
using Core.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Business
{
    public class AuthBusiness(
        IAuthService authService,
        IConfiguration configuration
        ) : IAuthBusiness
    {
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        public SignInResponseDto SignIn(SignInDto model)
        {
            try
            {
                string email = _configuration["auth:Email"] ?? string.Empty;
                string pass = _configuration["auth:Password"] ?? string.Empty;

                bool valid = false;
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(email) && model.Mail == email && model.Password == pass)
                { valid = true; }

                if (valid)
                {
                    TokenDto token = _authService.GenerateJwtToken(model.Mail);
                    return (new() { Token = token });
                }
                throw new Exception("AuthBusiness | SignIn | Invalid email or password!");
            }
            catch (Exception ex)
            {
                throw new Exception($"AuthBusiness | SignIn | {ex.Message}");
            }
        }
    }
}