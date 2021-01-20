using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserAccountDto
    {
        [Required(ErrorMessage = "The {0} field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        public string Password { get; set; }
    }
}
