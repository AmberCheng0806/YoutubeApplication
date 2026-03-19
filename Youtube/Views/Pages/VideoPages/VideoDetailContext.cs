//using Microsoft.Toolkit.Uwp.Notifications;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Components.PlayListComponent;
using Youtube.Utility;
using YoutubeAPI;
using YoutubeAPI.Video.Models;

namespace Youtube.Views.Pages.VideoPages
{
    [AddINotifyPropertyChangedInterface]
    internal class VideoDetailContext : INavigationAware
    {
        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string ChannelTitle { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ViewCount { get; set; }
        public string LikeCount { get; set; }
        public string ImageUrl { get; set; }
        public ObservableCollection<PlaylistItem> Playlists { get; set; } = new ObservableCollection<PlaylistItem>();
        public bool IsSubscript { get; set; } = false;
        public string RateText { get; set; } = "";
        [DependsOn(nameof(IsSubscript))]
        public string SubcriptionText => IsSubscript ? "已訂閱" : "訂閱";
        private string subscriptionId = "";
        private YoutubeContext youtubeContext = new YoutubeContext();
        private string PlayListItemId;
        public ICommand SubscriptCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand DisLikeCommand { get; set; }
        public ICommand SavePlaylistCommand { get; set; }
        public async void OnNavigatedTo(object[] parameter)
        {
            VideoId = parameter[0] as string;
            var videostatics = await youtubeContext.Video.GetByVideoIdAsync(VideoId);
            VideoTitle = videostatics.items[0].snippet.title;
            VideoDescription = videostatics.items[0].snippet.description;
            ChannelTitle = videostatics.items[0].snippet.channelTitle;
            PublishedAt = videostatics.items[0].snippet.publishedAt;
            ViewCount = videostatics.items[0].statistics.viewCount;
            LikeCount = videostatics.items[0].statistics.likeCount;
            string channelId = videostatics.items[0].snippet.channelId;
            var subscription = await youtubeContext.Subscription.GetAsync(channelId);
            if (subscription.items.Length != 0)
            {
                IsSubscript = true;
                subscriptionId = subscription.items[0].id;
            }
            var channel = await youtubeContext.Channel.GetByChannelIdAsync(channelId);
            ImageUrl = channel.items[0].snippet.thumbnails.medium.url;
            var rate = await youtubeContext.Video.GetRateAsync(VideoId);
            RateText = rate.items[0].rating;

            var playList = await youtubeContext.Playlist.GetAllAsync();
            foreach (var item in playList.items)
            {
                bool isAdded = await IsAddedPlaylist(item.id, VideoTitle);
                Playlists.Add(new PlaylistItem(item.id, item.snippet.title, isAdded));
            }

            SubscriptCommand = new RelayCommand(async () =>
            {
                IsSubscript = !IsSubscript;
                if (IsSubscript)
                {
                    var createSuscription = await youtubeContext.Subscription.SubscriptAsync(channelId);
                    subscriptionId = createSuscription.id;
                }
                else
                {
                    await youtubeContext.Subscription.DeleteAsync(subscriptionId);
                }
            });
            LikeCommand = new RelayCommand(async () =>
            {
                if (RateText == "like")
                {
                    RateText = "none";
                    await youtubeContext.Video.RateAsync(VideoId, "none");
                }
                else
                {
                    RateText = "like";
                    await youtubeContext.Video.RateAsync(VideoId, "like");
                }
            });
            DisLikeCommand = new RelayCommand(async () =>
            {
                if (RateText == "dislike")
                {
                    RateText = "none";
                    await youtubeContext.Video.RateAsync(VideoId, "none");
                }
                else
                {
                    RateText = "dislike";
                    await youtubeContext.Video.RateAsync(VideoId, "dislike");
                }
            });

            SavePlaylistCommand = new RelayCommand<PlaylistItem>(async x =>
            {
                if (x.IsAddedPlaylist)
                {
                    await youtubeContext.PlayListItem.DeleteAsync(PlayListItemId);
                    x.IsAddedPlaylist = false;
                    //new ToastContentBuilder()
                    //.AddText("已移除播放清單")
                    //.Show();
                }
                else
                {
                    var playlistItem = await youtubeContext.PlayListItem.CreateAsync(x.Id, VideoId);
                    x.IsAddedPlaylist = true;
                    PlayListItemId = playlistItem.id;
                    //new ToastContentBuilder()
                    //.AddText("已加入播放清單")
                    //.Show();
                }
            });
        }

        private async Task<bool> IsAddedPlaylist(string playlistId, string videoTitle)
        {
            var playlistItem = await youtubeContext.PlayListItem.GetAllAsync(playlistId);
            var list = playlistItem.items;
            return list.Any(x => { if (x.snippet.title == videoTitle) PlayListItemId = x.id; return x.snippet.title == videoTitle; });
        }
    }
}
