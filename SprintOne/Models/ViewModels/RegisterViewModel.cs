using System.ComponentModel.DataAnnotations;

namespace SprintOne.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username.")]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}