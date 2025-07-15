using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Models.Contracts
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
