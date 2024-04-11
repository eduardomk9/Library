using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Auth
{
    public class TokenDto
    {
        [Required]
        public string AccessToken { get; set; } = null!;
        [Required]
        public long ExpiresIn { get; set; }
        [Required]
        public string TokenType { get; set; } = null!;
    }
}
