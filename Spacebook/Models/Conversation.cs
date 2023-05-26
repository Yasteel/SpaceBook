namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Conversation
    {
        [Key]
        [Column("pkConversationId")]
        public int ConversationId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Column("fkParticipantOne")]
        public int? ParticipantOne { get; set; }

        [Column("fkParticipantTwo")]
        public int? ParticipantTwo { get; set; }

        
        /*[ForeignKey(nameof(this.ParticipantOne))]
        public Profile? ParticipantOneEntity { get; set; }

        [ForeignKey(nameof(this.ParticipantTwo))]
        public Profile? ParticipantTwoEntity { get; set; }*/
    }
}
