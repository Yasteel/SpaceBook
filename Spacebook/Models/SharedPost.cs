namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class SharedPost
    {
        [Key]
        [Column("pkSharedPostId")]
        public int SharedPostId { get; set; }

        [Column("fkPostId")]
        public int PostId { get; set; }

        [Column("fkProfileId")]
        public int ProfileId { get; set; }
    }
}
