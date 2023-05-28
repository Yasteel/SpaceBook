namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Post
    {
        [Key]
        [Column("pkPostId")]
        public int PostId { get; set; }

        [Column("fkProfileId")]
        public int ProfileId { get; set; }

        public string? Type { get; set; }

        public string? MediaUrl { get; set; }

        public string? Caption { get; set; }

        // Tags field in Database to be stored as a JSON string
        // so each post can have multiple tags
        public string? Tags { get; set; }

        public DateTime Timestamp { get; set; }

        public string AccessLevel { get; set; }

        [NotMapped]
        public string? SharedIDs { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public IFormFile? VideoFile { get; set; }

        /*// Name inside parenthesis here needs to be the same as the Variable Name
        [ForeignKey(nameof(this.ProfileId))] 
        public Profile? ProfileEntity { get; set; }*/

    }
}
