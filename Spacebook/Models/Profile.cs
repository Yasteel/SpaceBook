namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Profile
    {
        [Key]
        [Column("pkUserId")]
        public int? UserId { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Email { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime JoinedDate { get; set; }
    }
}