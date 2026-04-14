using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;
using YoutubeAPI;
using YoutubeAPI.Subscription.Models;
using static Youtube.Contracts.VideoDetailContract;

namespace Youtube.Presenters
{
    internal class VideoDetailPresenter : IVideoDetailPresenter
    {
        public IVideoDetailView View { get; set; }
        private YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        private string SubscriptionId { get; set; }
        private bool IsSubscribed { get; set; }
        private string RateText { get; set; } = "";
        private string ChannelId { get; set; }
        private string VideoTitle { get; set; }

        public VideoDetailPresenter(IVideoDetailView videoDetailView)
        {
            View = videoDetailView;
        }
        public async Task GetVideoDetailRequest(string videoId)
        {
            var videostatics = await YoutubeContext.Video.GetByVideoIdAsync(videoId);
            VideoTitle = videostatics.items[0].snippet.title;
            string videoDescription = videostatics.items[0].snippet.description;
            string channelTitle = videostatics.items[0].snippet.channelTitle;
            DateTime publishedAt = videostatics.items[0].snippet.publishedAt;
            string viewCount = videostatics.items[0].statistics.viewCount;
            string likeCount = videostatics.items[0].statistics.likeCount;
            ChannelId = videostatics.items[0].snippet.channelId;
            var subscription = await YoutubeContext.Subscription.GetAsync(ChannelId);
            if (subscription.items.Length != 0)
            {
                IsSubscribed = true;
                SubscriptionId = subscription.items[0].id;
            }
            var channel = await YoutubeContext.Channel.GetByChannelIdAsync(ChannelId);
            string imageUrl = channel.items[0].snippet.thumbnails.medium.url;
            var rate = await YoutubeContext.Video.GetRateAsync(videoId);
            RateText = rate.items[0].rating;
            View.RenderVideoDetail(new Models.VideoDetailDTO(VideoTitle, videoDescription, channelTitle, publishedAt, viewCount, likeCount, imageUrl, IsSubscribed, RateText));
        }

        public async Task SubscriptRequest()
        {
            var createSuscription = await YoutubeContext.Subscription.SubscriptAsync(ChannelId);
            SubscriptionId = createSuscription.id;
        }

        public async Task UnSubscriptRequest()
        {
            await YoutubeContext.Subscription.DeleteAsync(SubscriptionId);
        }

        public async Task RateVideoRequest(string videoId, string RateText)
        {
            await YoutubeContext.Video.RateAsync(videoId, RateText);
        }

        private async Task<(bool, string)> IsAddedPlaylist(string playlistId, string videoTitle)
        {
            var playlistItem = await YoutubeContext.PlayListItem.GetAllAsync(playlistId);
            var list = playlistItem.items;
            string playListItemVideoId = "";
            bool isAdded = list.Any(x =>
            {
                if (x.snippet.title == videoTitle)
                {
                    playListItemVideoId = x.id;
                }
                return x.snippet.title == videoTitle;
            });
            return (isAdded, playListItemVideoId);
        }

        public async Task GetPlayListRequest()
        {
            var playList = await YoutubeContext.Playlist.GetAllAsync();
            List<PlaylistItemDTO> playlistItemDTOs = new List<PlaylistItemDTO>();
            foreach (var item in playList.items)
            {
                (bool, string) isAddedResult = await IsAddedPlaylist(item.id, VideoTitle);
                playlistItemDTOs.Add(new PlaylistItemDTO(item.id, item.snippet.title, isAddedResult.Item1, isAddedResult.Item2));
            }
            View.RenderPlayList(playlistItemDTOs);
        }

        public async Task SavePlayListRequest(string playListId, string videoId)
        {
            var playlistItemVideo = await YoutubeContext.PlayListItem.CreateAsync(playListId, videoId);
            View.AddPlayListItemVideoId(playListId, playlistItemVideo.id);
        }

        public async Task RemovePlayListRequest(string playListItemVideoId)
        {
            await YoutubeContext.PlayListItem.DeleteAsync(playListItemVideoId);
            View.RemovePlayListItemVideoId(playListItemVideoId);
        }
    }
}
