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
            public const string TitleRequiredMessage = "The book {0} is required.";
            public const string TitleMaxLengthMessage = "The book {0} must not exceed {1} characters.";
            public const string TitleMinLengthMessage = "The book {0} should be at least {1} characters.";

            //Description
            public const string DescriptionRequiredMessage = "The book {0} is required.";
            public const string DescriptionMaxLengthMessage = "The {0} must not exceed {1} characters.";
            public const string DescriptionMinLengthMessage = "The {0} must be at least {1} characters.";

            //Author
            public const string AuthorRequiredMessage = "The book author name is required.";
            public const string AuthorMaxLengthMessage = "Author name must not exceed {1} characters.";
            public const string AuthorMinLengthMessage = "Author name must be at least {1} characters.";

            //Price
            public const string PriceRequiredMessage = "{0} is required.";
            public const string PriceNotInRangeMessage = "{0} must be a positive number.";

            //Pages
            public const string PagesRequiredMessage = "Number of {0} is required.";
            public const string PagesNotInRangeMessage = "{0} count must be a positive number.";

        }

        public static class GenreMessages
        {
            //Name
            public const string NameRequiredMessage = "The genre {0} is required.";
            public const string NameMaxLengthMessage = "The genre {0} must not exceed {1} characters.";
            public const string NameMinLengthMessage = "The genre {0} should be at least {1} characters.";

            //Description
            public const string DescriptionRequiredMessage = "The genre {0} is required.";
            public const string DescriptionMaxLengthMessage = "The {0} must not exceed {1} characters.";
            public const string DescriptionMinLengthMessage = "The {0} must be at least {1} characters.";

        }

        public static class LocationMessages
        {
            //CountryName
            public const string CountryNameRequiredMessage = "The location's country name is required.";
            public const string CountryNameMaxLengthMessage = "The location's country name must not exceed {1} characters.";
            public const string CountryNameMinLengthMessage = "The location's country name should be at least {1} characters.";

            //CityName
            public const string CityNameRequiredMessage = "The location's city name is required.";
            public const string CityNameMaxLengthMessage = "The location's city name must not exceed {1} characters.";
            public const string CityNameMinLengthMessage = "The location's city name should be at least {1} characters.";

            //ZipCode
            public const string ZipCodeRequiredMessage = "The location's ZIP Code is required.";
            public const string ZipCodeMaxLengthMessage = "The location's ZIP Code must not exceed {1} characters.";
            public const string ZipCodeMinLengthMessage = "The location's ZIP Code should be at least {1} characters.";

        }

        public static class ShopMessages
        {
            //Name
            public const string NameRequiredMessage = "The shop {0} is required.";
            public const string NameLengthMessage = "The shop {0} must be between {2} and {1} characters.";

            //Description
            public const string DescriptionRequiredMessage = "The shop {0} is required.";
            public const string DescriptionLengthMessage = "The shop {0} must be between {2} and {1} characters.";

        }

        public static class RoleMessages
        {
            //Name
            public const string NameLengthMessage = "The role {0} must be between {2} and {1} characters.";
            public const string NameRequiredMessage = "The role {0} is required.";
        }
    }
}
