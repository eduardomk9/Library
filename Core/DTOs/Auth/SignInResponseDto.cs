using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Auth
{
    public class SignInResponseDto
    {
        [Required]
        public TokenDto Token { get; set; } = null!;
    }
}
