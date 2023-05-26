namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Gender { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
