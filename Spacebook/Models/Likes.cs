namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Likes
    {
        [Key]
        [Column("pkLikeId")]
        public int LikeId { get; set; }

        [Column("fkProfileId")]
        public int? ProfileId { get; set; }

        [Column("fkPostId")]
        public int? PostId { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(this.ProfileId))]
        public Profile1? ProfileEntity { get; set; }

        [ForeignKey(nameof(this.PostId))]
        public Post? PostEntity { get; set; }
    }
}
