using Core.Dtos.Auth;

namespace Core.Services
{
    public interface IAuthService
    {
        TokenDto GenerateJwtToken(string userName);
    }
}
