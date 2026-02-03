using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    internal class VideoCardDTO
    {
        public string title { get; set; }
        public string url { get; set; }
        public string channelTitle { get; set; }
        public DateTime publishedAt { get; set; }
        public string viewCount { get; set; }
        public VideoCardDTO(string title, string url, string channelTitle, DateTime publishedAt, string viewCount)
        {
            this.title = title;
            this.url = url;
            this.channelTitle = channelTitle;
            this.publishedAt = publishedAt;
            this.viewCount = viewCount;
        }
    }
}
