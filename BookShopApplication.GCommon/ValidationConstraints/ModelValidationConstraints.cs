using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.GCommon.ValidationConstraints
{
    public static class ModelValidationConstraints
    {
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
    }
}
