﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data;
using BookShopApplication.Data.Models;
using BookShopApplication.Data.Repository.Contracts;
using BookShopApplication.Services.Contracts;
using BookShopApplication.Web.ViewModels;
using BookShopApplication.Web.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

namespace BookShopApplication.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IPurchaseItemUserRepository _purchaseItemUserRepository;

        public CartService(ICartRepository repository, IWishlistRepository wishlistRepository,IPurchaseItemUserRepository purchaseItemUserRepository)
        {
            this._cartRepository = repository;
            this._wishlistRepository = wishlistRepository;
            this._purchaseItemUserRepository = purchaseItemUserRepository;
        }


        public async Task<CartViewModel> DisplayAllCartItemsAsync(Guid userId,int page, int pageSize)
        {
            var cart = new CartViewModel();
            var cartItems =  _cartRepository.GetAllAttached()
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemViewModel
                {
                    BookId = c.BookId,
                    Id = c.Id,
                    ImagePath = c.Book.ImagePath,
                    Price = c.Book.Price.ToString("f2"),
                    Quantity = c.Quantity,
                    Title = c.Book.Title,
                    UserId = c.UserId
                });

            var items = await PaginatedList<CartItemViewModel>.CreateAsync(cartItems, page, pageSize);
            foreach (var item in cartItems)
            {
                cart.Total += decimal.Parse(item.Price) * item.Quantity;
            }
            cart.Items = items;
            
            return cart;
        }

        public async Task<bool> AddToCartAsync(Guid userId, Guid bookId)
        {

            bool alreadyExists = await _cartRepository
                .AnyAsync(w => w.UserId == userId && w.BookId == bookId);

            if (alreadyExists)
            {
                return false;
            }

            bool isInWishlist = await _wishlistRepository.AnyAsync(w => w.UserId == userId && w.BookId == bookId);
            
            if (isInWishlist)
            {
                var wishlistItem =
                    await _wishlistRepository.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);
                if (wishlistItem != null)
                {
                    if (!await _wishlistRepository.DeleteAsync(wishlistItem))
                    {
                        //Todo: custom error
                    } // calls SaveChangesAsync too.
                }
                else
                {
                    //Todo: custom error
                }


            }

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId,
                Quantity = 1,
            };

           return await _cartRepository.AddAsync(cartItem); // Calls SaveChangesAsync.
        }

        public async Task<bool> RemoveFromCartByIdAsync(Guid itemId)
        {
            bool isRemoved = false;

            var itemToRemove = await GetCartItemAsync(itemId, _cartRepository);
            if (itemToRemove != null)
            {
                isRemoved = await _cartRepository.DeleteAsync(itemToRemove); // Calls SaveChangesAsync.
                return isRemoved;
            }

            return isRemoved;
        }

        public async Task<bool> RemoveFromCartAsync(Guid userId, Guid itemId)
        {
            bool isRemoved = false;

            var itemToRemove = await GetCartItemByIdsAsync(userId, itemId, _cartRepository);
            if (itemToRemove != null)
            {
                
                isRemoved = await _cartRepository.DeleteAsync(itemToRemove);// Calls SaveChangesAsync.
                return isRemoved;
            }

            return isRemoved;
        }

        public async Task<bool> MoveToWishlistByIdAsync(Guid itemId)
        {
            bool isRemoved = false;
            bool isAdded = false;

            var itemToMove = await GetCartItemAsync(itemId, _cartRepository);

            

            var itemToAddToWishlist = new WishlistItem();

            if (itemToMove != null)
            {
                //Todo fix bug item that exists in wishlist to not be added.

                itemToAddToWishlist.Id = Guid.NewGuid();
                itemToAddToWishlist.BookId = itemToMove.BookId;
                itemToAddToWishlist.UserId = itemToMove.UserId;

                isRemoved = await _cartRepository.DeleteAsync(itemToMove); //Calls SaveChangesAsync.
            }

            if (itemToAddToWishlist != null)
            {
               isAdded = await _wishlistRepository.AddAsync(itemToAddToWishlist); // Calls SaveChangesAsync.
            }

            

            return isAdded && isRemoved;

        }

        public async Task<bool> PurchaseBooksAsync(ICollection<CartItemViewModel> books)
        {
            var itemsToPurchase = books.Select(b => new PurchaseItemUser
            {
                CartItemId = b.Id,
                UserId = b.UserId
            }).ToArray();

            bool removedFromCart = false;
            bool addedFromCart = false;
            foreach (var item in books)
            {
                var cartItem = await _cartRepository.GetByIdAsync(item.Id);
                if (cartItem != null)
                {
                    removedFromCart = await _cartRepository.SoftDeleteAsync(cartItem);

                    if (!removedFromCart)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            addedFromCart = await _purchaseItemUserRepository.AddRangeAsync(itemsToPurchase);
            return addedFromCart && removedFromCart;
        }


        // Private Helping methods...
        private static async Task<CartItem?> GetCartItemByIdsAsync(Guid userId, Guid itemId, ICartRepository cartRepository)
        {
            var item = await cartRepository.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == itemId);

            return item;
        }

        private static async Task<CartItem?> GetCartItemAsync(Guid itemId, ICartRepository cartRepository)
        {
            var item = await cartRepository.FirstOrDefaultAsync(w => w.Id == itemId);

            return item;

        }
    }
}
