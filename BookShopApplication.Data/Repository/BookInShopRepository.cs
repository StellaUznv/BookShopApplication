using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Repository
{
    public class BookInShopRepository : BaseRepository<BookInShop, (Guid BookId, Guid ShopId)> , IBookInShopRepository
    {
        public BookInShopRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
        public async Task<BookInShop?> GetByIdsAsync(Guid bookId, Guid shopId)
        {
            return await dbSet
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.ShopId == shopId);
        }
    }
}
