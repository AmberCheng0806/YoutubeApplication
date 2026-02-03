using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Enums;

namespace Youtube.Components.SearchFilterComponent
{
    public class SearchFilterDTO
    {
        public string Type { get; set; }
        public DateTime PublishedAfter { get; set; }
        public string Duration { get; set; }
        public VideoType VideoType { get; set; }

        public SearchFilterDTO(string type, DateTime publishedAfter, string duration, VideoType videoType)
        {
            Type = type;
            PublishedAfter = publishedAfter;
            Duration = duration;
            VideoType = videoType;
        }

        public SearchFilterDTO() { }
    }
}
