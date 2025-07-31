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
            public const string NameRequiredMessage = "The Genre {0} is required.";
            public const string NameLengthMessage = "The Genre's {0} must be between {2} and {1} characters.";

            //Description
            public const string DescriptionRequiredMessage = "The Genre {0} is required.";
            public const string DescriptionLengthMessage = "The Genre's {0} must be between {2} and {1} characters.";

        }

        public static class LocationMessages
        {
            //CountryName
            public const string CountryNameRequiredMessage = "The Location's Country Name is required.";
            public const string CountryNameLengthMessage = "The Location's Country Name must be between {2} and {1} characters.";
            

            //CityName
            public const string CityNameRequiredMessage = "The Location's City Name is required.";
            public const string CityNameLengthMessage = "The Location's City Name must be between {2} and {1} characters.";

            //ZipCode
            public const string ZipCodeRequiredMessage = "The Location's ZIP Code is required.";
            public const string ZipCodeLengthMessage = "The Location's ZIP Code must be between {2} and {1} characters.";

            //Address
            public const string AddressRequiredMessage = "The Location's Address is required.";
            public const string AddressLengthMessage = "The Location's Address must be between {2} and {1} characters.";

            //Longitude
            public const string LongitudeRequiredMessage = "The Location's Longitude is required.";

            //Latitude
            public const string LatitudeRequiredMessage = "The Location's Latitude is required.";

        }

        public static class ShopMessages
        {
            //Name
            public const string NameRequiredMessage = "The Shop {0} is required.";
            public const string NameLengthMessage = "The Shop {0} must be between {2} and {1} characters.";

            //Description
            public const string DescriptionRequiredMessage = "The Shop {0} is required.";
            public const string DescriptionLengthMessage = "The Shop {0} must be between {2} and {1} characters.";

        }

        public static class RoleMessages
        {
            //Name
            public const string NameLengthMessage = "The Role {0} must be between {2} and {1} characters.";
            public const string NameRequiredMessage = "The Role {0} is required.";
        }
    }
}
