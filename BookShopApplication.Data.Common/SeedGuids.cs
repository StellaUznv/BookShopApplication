using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Common
{
    public static class SeedGuids
    {
        // Genres
        public static readonly Guid Fantasy = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid SciFi = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid Mystery = Guid.Parse("33333333-3333-3333-3333-333333333333");

        // Books
        public static readonly Guid Book1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        public static readonly Guid Book2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        public static readonly Guid Book3 = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        public static readonly Guid Book4 = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        public static readonly Guid Book5 = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        public static readonly Guid Book6 = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        public static readonly Guid Book7 = Guid.Parse("11111111-2222-3333-4444-555555555555");

        // Locations
        public static readonly Guid Loc1 = Guid.Parse("99999999-0000-0000-0000-000000000001");
        public static readonly Guid Loc2 = Guid.Parse("99999999-0000-0000-0000-000000000002");
        public static readonly Guid Loc3 = Guid.Parse("99999999-0000-0000-0000-000000000003");

        // Shops
        public static readonly Guid Shop1 = Guid.Parse("44444444-4444-4444-4444-444444444444");
        public static readonly Guid Shop2 = Guid.Parse("55555555-5555-5555-5555-555555555555");
        public static readonly Guid Shop3 = Guid.Parse("66666666-6666-6666-6666-666666666666");

        // Users
        public static readonly Guid Manager = Guid.Parse("44444444-4444-4444-4444-444444444444");
       
    }
}
