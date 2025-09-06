﻿using BethanysPieShopNet8.Models;
using BethanysPieShopNet8.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopNet8.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly IShoppingCart _shoppingCart;
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(IPieRepository pieRepository,
                                      IShoppingCart shoppingCart,
                                      ILogger<ShoppingCartController> logger)
        {
            _pieRepository = pieRepository ?? throw new ArgumentNullException(nameof(pieRepository));
            _shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Loading the shopping cart page...");

            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            return View(shoppingCartViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int pieId)
        {
            _logger.LogInformation("Adding pie with ID: {pieId} to shopping cart...", pieId);

            var selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

            if (selectedPie != null)
            {
                _shoppingCart.AddToCart(selectedPie);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int pieId)
        {
            _logger.LogInformation("Removing pie with ID: {pieId} from shopping cart...", pieId);

            var selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);
            if (selectedPie != null)
            {
                _shoppingCart.RemoveFromCart(selectedPie);
            }
            return RedirectToAction("Index");
        }

    }
}
