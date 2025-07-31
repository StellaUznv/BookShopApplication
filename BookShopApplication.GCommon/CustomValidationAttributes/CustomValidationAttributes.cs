using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.GCommon.CustomValidationAttributes
{
    public static class CustomValidationAttributes
    {
        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;

            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var file = value as IFormFile;

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_extensions.Contains(extension))
                    {
                        return new ValidationResult($"This file type is not allowed. Allowed extensions: {string.Join(", ", _extensions)}");
                    }
                }

                return ValidationResult.Success;
            }
        }

        public class MaxFileSizeAttribute : ValidationAttribute
        {
            private readonly int _maxFileSizeInBytes;

            public MaxFileSizeAttribute(int maxFileSizeInBytes)
            {
                _maxFileSizeInBytes = maxFileSizeInBytes;
            }

            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var file = value as IFormFile;

                if (file != null && file.Length > _maxFileSizeInBytes)
                {
                    return new ValidationResult(string.Format(ErrorMessage!, _maxFileSizeInBytes / (1024 * 1024)));
                }

                return ValidationResult.Success;
            }
        }

    }
}
