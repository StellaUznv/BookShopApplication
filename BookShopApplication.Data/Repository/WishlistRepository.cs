using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Data.Repository
{
    public class WishlistRepository : BaseRepository<WishlistItem, Guid> , IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<List<Guid>> GetWishListedItemsIdsAsNoTrackingAsync(Guid userId)
        {
            var wishlistItems = await dbSet
                .Where(w => w.UserId == userId)
                .AsNoTracking()
                .Select(w => w.BookId)
                .ToListAsync();
            return wishlistItems;
        }
    }
}
