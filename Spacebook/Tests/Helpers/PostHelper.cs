namespace Spacebook.Tests.Helpers
{
    using Spacebook.Models;
    public static class PostHelper
    {
        public static Post CreatePost()
        {
            Post post = new Post()
            {
                SharedIDs = "1,2,3",
                Caption = "Test",
                AccessLevel = "public"
            };

            return post;
        }
    }
}
