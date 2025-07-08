using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.GCommon.ValidationErrorMessages
{
    public static class ModelErrorMessages
    {
        //Placeholders:
        //{0} - Name of field
        //{1} - First parameter value
        //{2} - Second parameter value
        public static class BookMessages
        {
            //Title
            public const string TitleRequired = "The book {0} is required.";
            public const string TitleMaxLength = "The book {0} must not exceed {1} characters.";
            public const string TitleMinLength = "The book {0} should be at least {1} characters.";

            //Description
            public const string DescriptionRequired = "The book {0} is required.";
            public const string DescriptionMaxLength = "The {0} must not exceed {1} characters.";
            public const string DescriptionMinLength = "The {0} must be at least {1} characters.";

            //Author
            public const string AuthorRequired = "The book author name is required.";
            public const string AuthorMaxLength = "Author name must not exceed {1} characters.";
            public const string AuthorMinLength = "Author name must be at least {1} characters.";

            //Price
            public const string PriceRequired = "{0} is required.";
            public const string PriceNotInRange = "{0} must be a positive number.";

            //Pages
            public const string PagesRequired = "Number of {0} is required.";
            public const string PagesNotInRange = "{0} count must be a positive number.";

        }
    }
}
