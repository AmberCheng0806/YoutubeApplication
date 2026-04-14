using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    internal class VideoDetailDTO
    {
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string ChannelTitle { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ViewCount { get; set; }
        public string LikeCount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsSubscript { get; set; } = false;
        public string RateText { get; set; } = "";

        public VideoDetailDTO(string videoTitle, string videoDescription, string channelTitle, DateTime publishedAt, string viewCount, string likeCount, string imageUrl, bool isSubscript, string rateText)
        {
            VideoTitle = videoTitle;
            VideoDescription = videoDescription;
            ChannelTitle = channelTitle;
            PublishedAt = publishedAt;
            ViewCount = viewCount;
            LikeCount = likeCount;
            ImageUrl = imageUrl;
            IsSubscript = isSubscript;
            RateText = rateText;
        }
        public VideoDetailDTO() { }
    }
}
