using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Youtube.Presenters.Models
{
    internal class MemberCenterVideoModel
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
        public MemberCenterVideoModel(string videoImg, string videoTitle, string videoDescription, string privacyStatus, DateTime publishedTime, string viewCount, string commentsCount, string likeCount, string disLikeCount)
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
        }
    }
}
