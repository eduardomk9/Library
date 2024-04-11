using System.ComponentModel.DataAnnotations;


namespace Core.DTOs.Book
{
    public class GetBooksDto
    {
        [Required]
        public string Type { get; set; } = null!;
        [Required]
        public string Value { get; set; } = null!;
    }
}
