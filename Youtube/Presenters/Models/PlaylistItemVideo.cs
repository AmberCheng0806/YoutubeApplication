using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube.Presenters.Models
{
    internal class PlaylistItemVideo
    {
        public string VideoImg { get; set; }
        public string VideoTitle { get; set; }
        public string ChannelName { get; set; }
        public string VideoId { get; set; }
        public string PlaylistVideoId { get; set; }

        public PlaylistItemVideo(string videoImg, string videoTitle, string channelName, string videoId, string playlistVideoId)
        {
            VideoImg = videoImg;
            VideoTitle = videoTitle;
            ChannelName = channelName;
            VideoId = videoId;
            PlaylistVideoId = playlistVideoId;
        }
    }
}
