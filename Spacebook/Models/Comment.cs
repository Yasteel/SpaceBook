namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment
    {
        [Key]
        [Column("pkCommentId")]
        public int CommentId { get; set; }

        [Column("fkOriginalPost")]
        public int? OriginalPost { get; set; }

        [Column("fkCommentPost")]
        public int? CommentPost { get; set; }



        [ForeignKey(nameof(this.OriginalPost))]
        public Post? OriginalEntity { get; set; }


        [ForeignKey(nameof(this.CommentPost))]
        public Post? CommentEntity { get; set; }

    }
}
