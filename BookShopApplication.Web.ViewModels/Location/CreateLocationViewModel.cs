using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookShopApplication.GCommon.ValidationConstraints.ModelValidationConstraints.LocationConstraints;
using static BookShopApplication.GCommon.ValidationErrorMessages.ModelErrorMessages.LocationMessages;

namespace BookShopApplication.Web.ViewModels.Location
{
    public class CreateLocationViewModel
    {
        [Required]
        public Guid Id = Guid.NewGuid();

        [Required(ErrorMessage = CountryNameRequiredMessage)]
        [StringLength(CountryNameMaxLength, ErrorMessage = CountryNameLengthMessage, MinimumLength = CountryNameMinLength)]
        public string CountryName { get; set; } = null!;

        [Required(ErrorMessage = CityNameRequiredMessage)]
        [StringLength(CityNameMaxLength, ErrorMessage = CityNameLengthMessage, MinimumLength = CityNameMinLength)]
        public string CityName { get; set; } = null!;

        [Required(ErrorMessage = AddressRequiredMessage)]
        [StringLength(AddressMaxLength, ErrorMessage = AddressLengthMessage, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = ZipCodeRequiredMessage)]
        [StringLength(ZipCodeMaxLength , ErrorMessage = ZipCodeLengthMessage, MinimumLength = ZipCodeMinLength)]
        public string ZipCode { get; set; } = null!;
        [Required(ErrorMessage = LatitudeRequiredMessage)]
        public double Latitude { get; set; }

        [Required(ErrorMessage = LongitudeRequiredMessage)]
        public double Longitude { get; set; }



    }
}
