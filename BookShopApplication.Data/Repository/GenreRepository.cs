using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;

namespace BookShopApplication.Data.Repository
{
    public class GenreRepository: BaseRepository<Genre,Guid>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
