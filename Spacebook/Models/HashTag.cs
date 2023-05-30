namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HashTag
    {
        [Key]
        [Column("pkHashTagId")]
        public int TagId { get; set; }

        [Column("fkPostId")]
        public int PostId { get; set; }

        public string? HashTagText { get; set; }

        /*// Name inside parenthesis here needs to be the same as the Variable Name
        [ForeignKey(nameof(this.PostId))]
        public Profile? PostEntity { get; set; }*/
    }
}
