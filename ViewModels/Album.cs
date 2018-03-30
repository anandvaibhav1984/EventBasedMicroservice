using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBasedMicroservice.ViewModels
{
    public class Album
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<AlbumItem> Data { get; set; }
    }
}
