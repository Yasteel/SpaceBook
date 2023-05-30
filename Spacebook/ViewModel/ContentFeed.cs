namespace Spacebook.ViewModel
{
    using Spacebook.Models;

    public class ContentFeed
    {
        public Post Post { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Likes> Likes { get; set; }

        public Profile Profile { get; set; }
    }
}
