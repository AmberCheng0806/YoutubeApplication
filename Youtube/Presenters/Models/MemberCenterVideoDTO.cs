using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    internal class MemberCenterVideoDTO
    {
        public string VideoImg { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string PrivacyStatus { get; set; }
        public DateTime PublishedTime { get; set; }
        public string ViewCount { get; set; }
        public string CommentsCount { get; set; }
        public string LikeCount { get; set; }
        public string DisLikeCount { get; set; }
        public string VideoId { get; set; }
        public string CategoryId { get; set; }
        public string OriginalVideoTitle { get; set; }
        public string OriginalVideoDescription { get; set; }
        public string OriginalVideoPrivacy { get; set; }

        public MemberCenterVideoDTO(string videoImg, string videoTitle, string videoDescription, string privacyStatus, DateTime publishedTime, string viewCount, string commentsCount, string likeCount, string disLikeCount, string videoId, string categoryId, string originalVideoTitle, string originalVideoDescription, string originalVideoPrivacy)
        {
            VideoImg = videoImg;
            VideoTitle = videoTitle;
            VideoDescription = videoDescription;
            PrivacyStatus = privacyStatus;
            PublishedTime = publishedTime;
            ViewCount = viewCount;
            CommentsCount = commentsCount;
            LikeCount = likeCount;
            DisLikeCount = disLikeCount;
            VideoId = videoId;
            CategoryId = categoryId;
            OriginalVideoTitle = originalVideoTitle;
            OriginalVideoDescription = originalVideoDescription;
            OriginalVideoPrivacy = originalVideoPrivacy;
        }
        public MemberCenterVideoDTO() { }
    }
}
