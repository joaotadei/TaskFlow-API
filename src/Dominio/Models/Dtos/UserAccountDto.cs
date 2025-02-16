using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.Dtos
{
    public class UserAccountDto
    {
        [Required(ErrorMessage = "The Email field is required"), Email]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        public string Password { get; set; }
    }
}