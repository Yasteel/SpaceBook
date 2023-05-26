namespace Spacebook
{
    using Microsoft.EntityFrameworkCore;
    using Spacebook.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Profile> Profile { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Likes> Likes { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Preference> Preference { get; set; }

        public DbSet<Conversation> Conversation { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<HashTag> HashTags { get; set; }
    }
}
