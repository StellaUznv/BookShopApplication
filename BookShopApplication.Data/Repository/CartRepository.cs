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
    public class CartRepository : BaseRepository<CartItem, Guid>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context)
            :base(context)
        {
        }

        public async Task<List<Guid>> GetCartItemsIdsAsNoTrackingAsync(Guid userId)
        {
            var cartItems = await dbSet
                .Where(c => c.UserId == userId)
                .AsNoTracking()
                .Select(c => c.BookId)
                .ToListAsync();
            return cartItems;
        }
    }
}
