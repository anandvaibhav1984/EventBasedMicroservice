using EventBasedMicroservice.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBasedMicroservice.Services
{
    public interface ICatalogService
    {
        Task<Album> GetCatalogItemsAsync(int page, int take, int? artist, int? genre);
        Task<IEnumerable<SelectListItem>> GetGenre();
        Task<IEnumerable<SelectListItem>> GetArtistAsync();
    }
}
