using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Spacebook.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string? Username { get; set; }

        public string? NotificationText { get; set; }
    }
}
