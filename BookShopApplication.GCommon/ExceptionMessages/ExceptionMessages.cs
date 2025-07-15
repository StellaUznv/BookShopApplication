using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookShopApplication.GCommon.ExceptionMessages
{
    public static class ExceptionMessages
    {
        public static class Data
        {
            public const string SoftDeleteOnNonSoftDeletableEntity =
                "Soft Delete can't be performed on an Entity which does not support it!";
        }
    }
}
