using System.ComponentModel.DataAnnotations;

namespace Planer.Models
{
    public class UpdateUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Password { get; set; }
    }
}
