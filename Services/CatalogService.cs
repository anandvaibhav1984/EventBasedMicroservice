using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventBasedMicroservice.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Resilience.Http;
using EventBasedMicroservice.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventBasedMicroservice.Services
{
    public class CatalogService:ICatalogService
    {
        private readonly IHttpClient _apiClient;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly string _remoteServiceBaseUrl;
        public CatalogService(IOptionsSnapshot<AppSettings> settings, IHttpClient  httpClient)
        {
            _apiClient = httpClient;
            _settings = settings;
            _remoteServiceBaseUrl = $"{_settings.Value.CatalogUrl}/api/v1/catalog/";
        }

        public async Task<IEnumerable<SelectListItem>> GetGenre()
        {
            var getTypesUri = API.Catalog.GetAllTypes(_remoteServiceBaseUrl);

            var dataString = await _apiClient.GetStringAsync(getTypesUri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var brands = JArray.Parse(dataString);
            foreach (var brand in brands.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("genreId"),
                    Text = brand.Value<string>("name")
                });
            }
            return items;
        }

        public async Task<Album> GetCatalogItemsAsync(int page, int take, int? artist, int? genre)
        {
            var allcatalogItemsUri = API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, page, take, artist, genre);

            var dataString = await _apiClient.GetStringAsync(allcatalogItemsUri);

            var response = JsonConvert.DeserializeObject<Album>(dataString);

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetArtistAsync()
        {
            var getBrandsUri = API.Catalog.GetAllBrands(_remoteServiceBaseUrl);

            var dataString = await _apiClient.GetStringAsync(getBrandsUri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var brands = JArray.Parse(dataString);

            foreach (var brand in brands.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Value<string>("artistId"),
                    Text = brand.Value<string>("name")
                });
            }

            return items;
        }
    }
}
