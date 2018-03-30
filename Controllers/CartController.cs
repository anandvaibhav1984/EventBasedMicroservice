using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBasedMicroservice.Services;
using EventBasedMicroservice.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace EventBasedMicroservice.Controllers
{
 
    public class CartController : Controller
    {
        private readonly IBasketService _basketSvc;
        private readonly ICatalogService _catalogSvc;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;

        public CartController(IBasketService basketSvc, ICatalogService catalogSvc, IIdentityParser<ApplicationUser> appUserParser)
        {
            _basketSvc = basketSvc;
            _catalogSvc = catalogSvc;
            _appUserParser = appUserParser;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = _appUserParser.Parse(HttpContext.User);
                var vm = await _basketSvc.GetBasket(user);

                return View(vm);
            }
            catch (BrokenCircuitException)
            {
                // Catch error when Basket.api is in circuit-opened mode                 
                HandleBrokenCircuitException();
            }

            return View();
        }

        public async Task<IActionResult> AddToCart(AlbumItem productDetails)
        {
            try
            {
                if ( productDetails.AlbumId > 0)
                {
                    var user = _appUserParser.Parse(HttpContext.User);
                    var product = new BasketItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        ProductName = productDetails.Title,
                        PictureUrl = productDetails.AlbumArtUrl,
                        UnitPrice = productDetails.Price,
                        ProductId = productDetails.AlbumId.ToString()
                    };
                    await _basketSvc.AddItemToBasket(user, product);
                }
                return RedirectToAction("Index", "Catalog");
            }
            catch (BrokenCircuitException)
            {
                // Catch error when Basket.api is in circuit-opened mode                 
                HandleBrokenCircuitException();
            }

            return RedirectToAction("Index", "Catalog", new { errorMsg = ViewBag.BasketInoperativeMsg });
        }
        private void HandleBrokenCircuitException()
        {
            ViewBag.BasketInoperativeMsg = "Basket Service is inoperative, please try later on. (Business Msg Due to Circuit-Breaker)";
        }
    }
}