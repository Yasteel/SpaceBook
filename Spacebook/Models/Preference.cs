namespace Spacebook.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Preference
	{
		[Key]
        public int Id { get; set; }

		[Column("fkProfileId")]
		public int ProfileId { get; set; }

        public string? Preferences { get; set; }


        /*[ForeignKey(nameof(this.ProfileId))]
		public Profile? ProfileEntity { get; set; }*/
	}
}
