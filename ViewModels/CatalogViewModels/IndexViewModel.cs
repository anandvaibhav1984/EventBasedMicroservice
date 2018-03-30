using EventBasedMicroservice.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBasedMicroservice.ViewModels.CatalogViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<AlbumItem> CatalogItems { get; set; }
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Artist { get; set; }
        public IEnumerable<SelectListItem> Genre { get; set; }
        public int? ArtistFilterApplied { get; set; }
        public int? GenreFilterApplied { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
