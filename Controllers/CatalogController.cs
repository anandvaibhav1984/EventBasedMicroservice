using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBasedMicroservice.Services;
using EventBasedMicroservice.ViewModels.CatalogViewModels;
using EventBasedMicroservice.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace EventBasedMicroservice.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService _catalogSvc;
        public CatalogController(ICatalogService catalogSvc)
        {
            _catalogSvc = catalogSvc;
        }



        public async Task<IActionResult> Index(int? ArtistFilterApplied, int? GenreFilterApplied, int? page, [FromQuery]string errorMsg)
        {
            var itemsPage = 10;
            var catalog = await _catalogSvc.GetCatalogItemsAsync(page ?? 0, itemsPage, ArtistFilterApplied, GenreFilterApplied);
            var vm = new IndexViewModel()
            {
                CatalogItems = catalog.Data,
                Artist = await _catalogSvc.GetArtistAsync(),
                Genre= await _catalogSvc.GetGenre(),
                ArtistFilterApplied = ArtistFilterApplied ?? 0,
                GenreFilterApplied = GenreFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = catalog.Data.Count,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)catalog.Count / itemsPage))
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            ViewBag.BasketInoperativeMsg = errorMsg;

            return View(vm);
        }
    }
}