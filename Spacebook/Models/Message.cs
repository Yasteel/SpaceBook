﻿namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Message
    {
        [Key]
        [Column("pkMessageId")]
        public int MessageId { get; set; }


        [Column("fkConversationId")]
        public int? ConversationId { get; set; }

        [Column("fkSenderId")]
        public int? SenderId { get; set; }

        public string? MessageType { get; set; }

        public string? Content { get; set; }

        public string? MessageImageUrl { get; set; }

        [NotMapped]
        public IFormFile? MessageImage { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Seen { get; set; }


        /*[ForeignKey(nameof(this.ConversationId))]
        public Conversation? ConversationEntity { get; set; }

        [ForeignKey(nameof(this.SenderId))]
        public Profile? ProfileEntity { get; set; }*/
    }
}
