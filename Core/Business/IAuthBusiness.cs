using Core.Dtos.Auth;

namespace Core.Business
{
    public interface IAuthBusiness
    {
        SignInResponseDto SignIn(SignInDto model);
    }
}
