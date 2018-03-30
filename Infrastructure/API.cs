using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBasedMicroservice.Infrastructure
{

    public static class API
    {
        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }

            public static string UpdateBasket(string baseUri)
            {
                return baseUri;
            }

            public static string CheckoutBasket(string baseUri)
            {
                return $"{baseUri}/checkout";
            }

            public static string CleanBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }

        public static class Order
        {
            public static string GetOrder(string baseUri, string orderId)
            {
                return $"{baseUri}/{orderId}";
            }

            public static string GetAllMyOrders(string baseUri)
            {
                return baseUri;
            }

            public static string AddNewOrder(string baseUri)
            {
                return $"{baseUri}/new";
            }

            public static string CancelOrder(string baseUri)
            {
                return $"{baseUri}/cancel";
            }

            public static string ShipOrder(string baseUri)
            {
                return $"{baseUri}/ship";
            }
        }

        public static class Catalog
        {
            public static string GetAllCatalogItems(string baseUri, int page, int take, int? artist, int? genre)
            {
                var filterQs = "";

                if (artist.HasValue || genre.HasValue)
                {
                    var brandQs = (artist.HasValue) ? genre.Value.ToString() : "null";
                    var typeQs = (genre.HasValue) ? genre.Value.ToString() : "null";
                    filterQs = $"/type/{typeQs}/brand/{brandQs}";
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetAllBrands(string baseUri)
            {
                return $"{baseUri}artistBrands";
            }

            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}genreTypes";
            }
        }

        public static class Marketing
        {
            public static string GetAllCampaigns(string baseUri, int take, int page)
            {
                return $"{baseUri}user?pageSize={take}&pageIndex={page}";
            }

            public static string GetAllCampaignById(string baseUri, int id)
            {
                return $"{baseUri}{id}";
            }
        }

        public static class Locations
        {
            public static string CreateOrUpdateUserLocation(string baseUri)
            {
                return baseUri;
            }
        }
    }
}
