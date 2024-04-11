using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Auth
{
    public class SignInDto
    {
        [Required]
        public string Mail { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
