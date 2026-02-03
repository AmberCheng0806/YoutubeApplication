using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Components.VideoCardComponent
{
    public class VideoCardContext
    {
        public string title { get; set; }
        public string url { get; set; }
        public string channelTitle { get; set; }
        public DateTime publishedAt { get; set; }
        public string viewCount { get; set; }
    }
}
