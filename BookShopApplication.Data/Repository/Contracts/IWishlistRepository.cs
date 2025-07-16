using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface IWishlistRepository : IRepository<WishlistItem, Guid> 
        , IAsyncRepository<WishlistItem, Guid>
    {
        public Task<List<Guid>> GetWishListedItemsIdsAsNoTrackingAsync(Guid userId);
    }
}
