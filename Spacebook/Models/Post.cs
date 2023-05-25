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

        public string? Media { get; set; }

        public string? Tags { get; set; }

        public DateTime Timestamp { get; set; }

        // Name inside parenthesis here needs to be the same as the Variable Name
        [ForeignKey(nameof(this.ProfileId))] 
        public Profile? ProfileEntity { get; set; }

    }
}
