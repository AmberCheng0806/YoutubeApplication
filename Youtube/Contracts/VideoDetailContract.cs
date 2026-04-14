using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;

namespace Youtube.Contracts
{
    internal class VideoDetailContract
    {
        public interface IVideoDetailView
        {
            void RenderVideoDetail(VideoDetailDTO videoDetailDTO);
            void RenderPlayList(List<PlaylistItemDTO> playlistItemDTOs);
            void AddPlayListItemVideoId(string playListId, string playListItemVideoId);
            void RemovePlayListItemVideoId(string playListItemVideoId);
        }

        public interface IVideoDetailPresenter
        {
            Task GetVideoDetailRequest(string videoId);
            Task SubscriptRequest();
            Task UnSubscriptRequest();
            Task RateVideoRequest(string videoId, string RateText);
            Task GetPlayListRequest();
            Task SavePlayListRequest(string playListId, string videoId);
            Task RemovePlayListRequest(string playListItemVideoId);
        }
    }
}
