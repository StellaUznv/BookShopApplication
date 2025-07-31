using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.GCommon.ValidationConstraints
{
    public static class ModelValidationConstraints
    {
        public static class UserConstraints
        {
            //FirstName
            public const int FirstNameMaxLength = 100;
            public const int FirstNameMinLength = 3;

            //LastName
            public const int LastNameMaxLength = 100;
            public const int LastNameMinLength = 4;
        }

        public static class BookConstraints
        {
            //Title
            public const int TitleMaxLength = 150;
            public const int TitleMinLength = 3;

            //Description
            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 3;

            //Author
            public const int AuthorNameMaxLength = 150;
            public const int AuthorNameMinLength = 3;

            //Price
            public const int PriceMinValue = 0;
            public const int PriceMaxValue = int.MaxValue;

            //Pages
            public const int PagesMaxValue = int.MaxValue;
            public const int PagesMinValue = 0;

            //Image
            public const int ImageMaxLength = 1000;
        }

        public static class GenreConstraints
        {
            //Name
            public const int NameMaxLength = 150;
            public const int NameMinLength = 3;

            //Description
            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 3;
        }

        public static class LocationConstraints
        {
            //Country Name
            public const int CountryNameMaxLength = 150;
            public const int CountryNameMinLength = 3;

            //City Name
            public const int CityNameMaxLength = 150;
            public const int CityNameMinLength = 3;

            //Address
            public const int AddressMaxLength = 500;
            public const int AddressMinLength = 3;

            //ZipCode

            public const int ZipCodeMaxLength = 20;
            public const int ZipCodeMinLength = 3;
        }

        public static class ShopConstraints
        {
            //Name
            public const int NameMaxLength = 150;
            public const int NameMinLength = 3;

            //Description
            public const int DescriptionMaxLength = 1000;
            public const int DescriptionMinLength = 3;
        }

        public static class RoleConstraints
        {
            //Name
            public const int NameMaxLength = 256;
            public const int NameMinLength = 3;
        }
    }
}
