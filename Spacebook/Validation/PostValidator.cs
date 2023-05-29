namespace Spacebook.Validation
{
    using FluentValidation;

    using Spacebook.Models;

    using System.Collections.Generic;

    public class PostValidator: AbstractValidator<Post>
    {
        public PostValidator() 
        { 

            this.RuleFor(_ => _.Caption)
                .NotEmpty();

            this.RuleFor(_ => _.AccessLevel)
                .NotEmpty();

            this.RuleFor(_ => _.VideoFile)
                .Must(ValidateVideoFile);

            this.RuleFor(_ => _.ImageFile)
                .Must(ValidateImageFile);

            this.RuleFor(_ => _.SharedIDs)
                .Must(ValidateSharedIDs);
        }

        private bool ValidateVideoFile(IFormFile? file)
        {
            if (file == null)
            {
                return true;
            }

            if (file.Length / ((1024 * 1024)) > 20)
            {
                return false;
            }
            else if ( file.ContentType != "video/mp4")
            {
                return false;
            }

            return true;
        }

        private bool ValidateImageFile(IFormFile? file)
        {
            if (file == null)
            {
                return true;
            }

            if (file.Length / ((1024 * 1024)) > 20)
            {
                return false;
            }
            else if (file.ContentType != "image/jpeg")
            {
                return false;
            }

            return true;
        }

        private bool ValidateSharedIDs(string? ids) 
        {
            if (ids == null)
            {
                return true;
            }

            var idList = ids.Split(',');

            int testVal = 0;
            var isAllInts = idList.All(_ => int.TryParse(_, out testVal));

            return isAllInts;
        }
    }
}
