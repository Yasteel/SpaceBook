namespace Spacebook.Tests
{
    using FluentValidation.TestHelper;

    using Spacebook.Interfaces;
    using Spacebook.Tests.Helpers;
    using Spacebook.Validation;
    using Xunit;

    public class PostValidatorTests
    {
        private readonly PostValidator validator;

        public PostValidatorTests()
        {
            validator = new PostValidator();
        }

        [Fact]
        public void ShouldValidate_Post_CorrectSharedIDs()
        {
            var post = PostHelper.CreatePost();
            var result = validator.TestValidate(post);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldValidate_Post_NullSharedId()
        {
            var post = PostHelper.CreatePost();
            post.SharedIDs = null;
            var result = validator.TestValidate(post);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldNotValidate_Post_InCorrectFormatSharedIDs()
        {
            var post = PostHelper.CreatePost();
            post.SharedIDs = "a,b,1";

            var result = validator.TestValidate(post);

            result.ShouldHaveValidationErrorFor(_ => _.SharedIDs);
        }
    }
}
