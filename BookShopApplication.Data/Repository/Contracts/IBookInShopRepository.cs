using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface IBookInShopRepository : IRepository<BookInShop, (Guid BookId, Guid ShopId)>
        , IAsyncRepository<BookInShop, (Guid BookId, Guid ShopId)>
    {
       public Task<BookInShop?> GetByIdsAsync(Guid bookId, Guid shopId);
    }
}
