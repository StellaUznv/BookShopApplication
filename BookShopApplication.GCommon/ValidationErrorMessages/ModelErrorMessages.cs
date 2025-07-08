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

        public static class GenreMessages
        {
            //Name
            public const string NameRequired = "The genre {0} is required.";
            public const string NameMaxLength = "The genre {0} must not exceed {1} characters.";
            public const string NameMinLength = "The genre {0} should be at least {1} characters.";

            //Description
            public const string DescriptionRequired = "The genre {0} is required.";
            public const string DescriptionMaxLength = "The {0} must not exceed {1} characters.";
            public const string DescriptionMinLength = "The {0} must be at least {1} characters.";

        }

        public static class LocationMessages
        {
            //CountryName
            public const string CountryNameRequired = "The location's country name is required.";
            public const string CountryNameMaxLength = "The location's country name must not exceed {1} characters.";
            public const string CountryNameMinLength = "The location's country name should be at least {1} characters.";

            //CityName
            public const string CityNameRequired = "The location's city name is required.";
            public const string CityNameMaxLength = "The location's city name must not exceed {1} characters.";
            public const string CityNameMinLength = "The location's city name should be at least {1} characters.";

            //ZipCode
            public const string ZipCodeRequired = "The location's ZIP Code is required.";
            public const string ZipCodeMaxLength = "The location's ZIP Code must not exceed {1} characters.";
            public const string ZipCodeMinLength = "The location's ZIP Code should be at least {1} characters.";

        }

        public static class ShopMessages
        {
            //Name
            public const string NameRequired = "The shop {0} is required.";
            public const string NameMaxLength = "The shop {0} must not exceed {1} characters.";
            public const string NameMinLength = "The shop {0} should be at least {1} characters.";

            //Description
            public const string DescriptionRequired = "The shop {0} is required.";
            public const string DescriptionMaxLength = "The {0} must not exceed {1} characters.";
            public const string DescriptionMinLength = "The {0} must be at least {1} characters.";

        }
    }
}
