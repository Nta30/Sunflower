using System.ComponentModel.DataAnnotations;

namespace Sunflower.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Username")]
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
