using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Youtube.Presenters.Enums;

namespace Youtube.Presenters.Models
{
    public class SearchRequestDTO
    {
        public string keyword { get; set; }
        public string type { get; set; }
        public DateTime publishedAfter { get; set; }
        public string videoDuration { get; set; }
        public VideoType videoCategoryId { get; set; }
        public SearchRequestDTO(string keyword, string type, DateTime publishedAfter, string videoDuration, VideoType videoCategoryId)
        {
            this.keyword = keyword;
            this.type = string.IsNullOrEmpty(type) ? "video" : type;
            this.publishedAfter = publishedAfter == default ? default : publishedAfter;
            this.videoDuration = string.IsNullOrEmpty(videoDuration) ? "any" : videoDuration;
            this.videoCategoryId = videoCategoryId == VideoType.全選 ? VideoType.全選 : videoCategoryId;
        }
    }
}
