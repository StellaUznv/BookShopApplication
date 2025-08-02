using BookShopApplication.Data;
using static BookShopApplication.Data.Seeding.Seeding;
using BookShopApplication.Data.Common;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.GCommon.EmailSender;
using BookShopApplication.Services;
using BookShopApplication.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddRazorPages();

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IShopRepository, ShopRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();
            builder.Services.AddScoped<IBookInShopRepository, BookInShopRepository>();
            builder.Services.AddScoped<IPurchaseItemUserRepository, PurchaseItemUserRepository>();

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IRoleService, RoleService>();

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Error/403";
                options.LoginPath = "/Account/Login"; // optional
            });


            var app = builder.Build();

            // SEED ROLES AND ADMIN USER HERE
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAndAdminAsync(services);
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //// Handles all exceptions
                //app.UseExceptionHandler("/Error");

                //// Handles non-matched routes (404s etc.)
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");

                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseExceptionHandler("/Error"); // Generic Error() action
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            app.MapRazorPages();

            app.Run();
        }
    }
}
