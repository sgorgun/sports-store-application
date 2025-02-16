﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IStoreRepository repository;

        public CartController(IStoreRepository repository, Cart cart)
        {
            this.repository = repository;
            this.Cart = cart;
        }

        public Cart Cart { get; set; }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            return View(new CartViewModel
            {
                ReturnUrl = returnUrl ?? "/",
                Cart = this.Cart
            });
        }

        [HttpPost]
        [Route("Cart/Remove")]
        public IActionResult Remove(long productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
            return View("Index", new CartViewModel
            {
                Cart = Cart,
                ReturnUrl = returnUrl ?? "/"
            });
        }


        [HttpPost]
        public IActionResult Index(long productId, string returnUrl)
        {
            Product? product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                this.Cart.AddItem(product, 1);

                return View(new CartViewModel
                {
                    Cart = this.Cart,
                    ReturnUrl = returnUrl
                });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
