//using Microsoft.Toolkit.Uwp.Notifications;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.Notifications;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Components.CommentComponent;
using Youtube.Components.PlayListComponent;
using Youtube.Components.ReplyCommentComponent;
using Youtube.Presenters;
using Youtube.Presenters.Models;
using Youtube.Utility;
using YoutubeAPI;
using YoutubeAPI.Video.Models;
using static Youtube.Contracts.CommentContract;
using static Youtube.Contracts.VideoDetailContract;

namespace Youtube.Views.Pages.VideoPages
{
    [AddINotifyPropertyChangedInterface]
    internal class VideoDetailContext : INavigationAware, ICommentView, IVideoDetailView
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
        [AlsoNotifyFor(nameof(NoCommentsVisibility))]
        public ObservableCollection<CommentItem> Comments { get; set; } = new ObservableCollection<CommentItem>();
        public bool IsSubscript { get; set; } = false;
        public string RateText { get; set; } = "";
        //public string CommentText { get; set; }
        public Visibility NoCommentsVisibility => Comments.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        [DependsOn(nameof(IsSubscript))]
        public string SubcriptionText => IsSubscript ? "已訂閱" : "訂閱";
        public int TotalComments { get; set; }
        //private string subscriptionId = "";
        private YoutubeContext youtubeContext = new YoutubeContext();
        private ICommentPresenter CommentPresenter;
        private IVideoDetailPresenter VideoDetailPresenter;
        //private bool IsInitiateReply { get; set; } = false;
        public ICommand SubscriptCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand DisLikeCommand { get; set; }
        public ICommand SavePlaylistCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public ICommand AddReplyCommentCommand { get; set; }
        public ICommand ShowReplyCommentsCommand { get; set; }
        public ICommand ClearTopReplyCommentTextCommand { get; set; }
        public VideoDetailContext()
        {
            CommentPresenter = new CommentPresenter(this);
            VideoDetailPresenter = new VideoDetailPresenter(this);
        }
        public void OnNavigatedTo(object[] parameter)
        {
            VideoId = parameter[0] as string;
            VideoDetailPresenter.GetVideoDetailRequest(VideoId);
            //var videostatics = await youtubeContext.Video.GetByVideoIdAsync(VideoId);
            //VideoTitle = videostatics.items[0].snippet.title;
            //VideoDescription = videostatics.items[0].snippet.description;
            //ChannelTitle = videostatics.items[0].snippet.channelTitle;
            //PublishedAt = videostatics.items[0].snippet.publishedAt;
            //ViewCount = videostatics.items[0].statistics.viewCount;
            //LikeCount = videostatics.items[0].statistics.likeCount;
            //string channelId = videostatics.items[0].snippet.channelId;
            //var subscription = await youtubeContext.Subscription.GetAsync(channelId);
            //if (subscription.items.Length != 0)
            //{
            //    IsSubscript = true;
            //    subscriptionId = subscription.items[0].id;
            //}
            //var channel = await youtubeContext.Channel.GetByChannelIdAsync(channelId);
            //ImageUrl = channel.items[0].snippet.thumbnails.medium.url;
            //var rate = await youtubeContext.Video.GetRateAsync(VideoId);
            //RateText = rate.items[0].rating;

            //var playList = await youtubeContext.Playlist.GetAllAsync();
            //foreach (var item in playList.items)
            //{
            //    bool isAdded = await IsAddedPlaylist(item.id, VideoTitle);
            //    Playlists.Add(new PlaylistItem(item.id, item.snippet.title, isAdded));
            //}
            VideoDetailPresenter.GetPlayListRequest();

            SubscriptCommand = new RelayCommand(() =>
            {
                IsSubscript = !IsSubscript;
                if (IsSubscript)
                {
                    //var createSuscription = await youtubeContext.Subscription.SubscriptAsync(channelId);
                    //subscriptionId = createSuscription.id;
                    VideoDetailPresenter.SubscriptRequest();
                }
                else
                {
                    //await youtubeContext.Subscription.DeleteAsync(subscriptionId);
                    VideoDetailPresenter.UnSubscriptRequest();
                }
            });
            LikeCommand = new RelayCommand(() =>
            {
                if (RateText == "like")
                {
                    RateText = "none";
                    VideoDetailPresenter.RateVideoRequest(VideoId, RateText);
                }
                else
                {
                    RateText = "like";
                    VideoDetailPresenter.RateVideoRequest(VideoId, RateText);
                }
            });
            DisLikeCommand = new RelayCommand(() =>
            {
                if (RateText == "dislike")
                {
                    RateText = "none";
                    VideoDetailPresenter.RateVideoRequest(VideoId, RateText);
                }
                else
                {
                    RateText = "dislike";
                    VideoDetailPresenter.RateVideoRequest(VideoId, RateText);
                }
            });

            SavePlaylistCommand = new RelayCommand<PlaylistItem>(x =>
            {
                if (x.IsAddedVideo)
                {
                    //await youtubeContext.PlayListItem.DeleteAsync(PlayListItemId);
                    VideoDetailPresenter.RemovePlayListRequest(x.PlayListItemVideoId);
                    x.IsAddedVideo = false;
                    new ToastContentBuilder()
                    .AddText("已移除播放清單")
                    .Show();
                }
                else
                {
                    //var playlistItem = await youtubeContext.PlayListItem.CreateAsync(x.Id, VideoId);
                    VideoDetailPresenter.SavePlayListRequest(x.Id, VideoId);
                    x.IsAddedVideo = true;
                    //PlayListItemId = playlistItem.id;
                    new ToastContentBuilder()
                    .AddText("已加入播放清單")
                    .Show();
                }
            });

            //var comments = await youtubeContext.Comment.GetCommentByVideoIdAsync(VideoId);
            //var items = comments?.items.Select(item =>
            //{
            //    bool isLiked = item.snippet.topLevelComment.snippet.viewerRating == "none" ? false : true;
            //    return new CommentItem(item.snippet.topLevelComment.snippet.authorDisplayName, item.snippet.topLevelComment.snippet.textDisplay, item.id, item.snippet.topLevelComment.snippet.authorProfileImageUrl,
            //        item.snippet.topLevelComment.snippet.likeCount, isLiked, item.snippet.topLevelComment.snippet.publishedAt);
            //});
            //Comments = new ObservableCollection<CommentItem>(items);
            CommentPresenter.LoadCommentsRequest(VideoId);

            AddCommentCommand = new RelayCommand<ReplyCommentContext>(x =>
            {
                //var createComment = await youtubeContext.Comment.CreateCommentByVideoIdAsync(VideoId, commentText);
                //bool isLiked = createComment.snippet.topLevelComment.snippet.viewerRating == "none" ? false : true;
                //Comments.Add(new CommentItem(createComment.snippet.topLevelComment.snippet.authorDisplayName, createComment.snippet.topLevelComment.snippet.textDisplay, createComment.id, createComment.snippet.topLevelComment.snippet.authorProfileImageUrl,
                //    createComment.snippet.topLevelComment.snippet.likeCount, isLiked, createComment.snippet.topLevelComment.snippet.publishedAt));
                CommentPresenter.AddCommentRequest(VideoId, x.CommentText);
                x.CommentText = "";
            });
            WeakReferenceMessenger.Default.Register<CommentItemDTO>(this, (sender, dto) =>
            {
                TotalComments++;
            });
            AddReplyCommentCommand = new RelayCommand<ReplyCommentContext>(x =>
            {
                CommentPresenter.AddReplyCommentRequest(x.ParentId, x.CommentText);
                x.CommentText = "";
            });
            ShowReplyCommentsCommand = new RelayCommand<CommentItem>(x =>
            {
                x.ReplyCommentsCollectionVisibility =
                    x.ReplyCommentsCollectionVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                if (x.ReplyCommentsCollectionVisibility == Visibility.Visible && x.IsInitiateReply == false)
                {
                    CommentPresenter.LoadReplyCommentsRequest(x.Id);
                }
            });
            ClearTopReplyCommentTextCommand = new RelayCommand<ReplyCommentContext>(x => { x.CommentText = ""; });
        }

        //private async Task<bool> IsAddedPlaylist(string playlistId, string videoTitle)
        //{
        //    var playlistItem = await youtubeContext.PlayListItem.GetAllAsync(playlistId);
        //    var list = playlistItem.items;
        //    return list.Any(x => { if (x.snippet.title == videoTitle) PlayListItemId = x.id; return x.snippet.title == videoTitle; });
        //}

        public void RenderComments(List<CommentItemDTO> comments)
        {
            var commentItems = AutoMapper.AutoMapper.Map<CommentItemDTO, CommentItem>(comments).ToList();
            Comments = new ObservableCollection<CommentItem>(commentItems);
            CommentPresenter.LoadCommentsRatingRequest(comments);
        }

        public void AddComment(CommentItemDTO comment)
        {
            var commentItem = AutoMapper.AutoMapper.Map<CommentItemDTO, CommentItem>(comment);
            Comments.Insert(0, commentItem);
            WeakReferenceMessenger.Default.Send<CommentItemDTO>(comment);
        }

        public void RenderVideoDetail(VideoDetailDTO videoDetailDTO)
        {
            VideoTitle = videoDetailDTO.VideoTitle;
            VideoDescription = videoDetailDTO.VideoDescription;
            ChannelTitle = videoDetailDTO.ChannelTitle;
            PublishedAt = videoDetailDTO.PublishedAt;
            ViewCount = videoDetailDTO.ViewCount;
            LikeCount = videoDetailDTO.LikeCount;
            ImageUrl = videoDetailDTO.ImageUrl;
            IsSubscript = videoDetailDTO.IsSubscript;
            RateText = videoDetailDTO.RateText;
        }

        public void RenderPlayList(List<PlaylistItemDTO> playlistItemDTOs)
        {
            var playlists = AutoMapper.AutoMapper.Map<PlaylistItemDTO, PlaylistItem>(playlistItemDTOs);
            Playlists = new ObservableCollection<PlaylistItem>(playlists);
        }

        public void AddPlayListItemVideoId(string playListId, string playListItemVideoId)
        {
            Playlists.FirstOrDefault(x =>
            {
                if (x.Id == playListId) x.PlayListItemVideoId = playListItemVideoId;
                return x.Id == playListId;
            });
        }

        public void RemovePlayListItemVideoId(string playListItemVideoId)
        {
            Playlists.FirstOrDefault(x =>
            {
                if (x.PlayListItemVideoId == playListItemVideoId) x.PlayListItemVideoId = "";
                return x.PlayListItemVideoId == playListItemVideoId;
            });
        }

        public void AddReplyComment(string parentId, CommentItemDTO comment)
        {
            var commentItem = AutoMapper.AutoMapper.Map<CommentItemDTO, CommentItem>(comment);
            Comments.FirstOrDefault(x =>
            {
                if (x.Id == parentId) { x.ReplyCommentItems.Insert(0, commentItem); x.ReplyCount++; };
                return x.Id == commentItem.Id;
            });
            WeakReferenceMessenger.Default.Send<CommentItemDTO>(comment);
        }

        public void RenderReplyComments(string parentId, List<CommentItemDTO> comments)
        {
            var replyComments = AutoMapper.AutoMapper.Map<CommentItemDTO, CommentItem>(comments).ToList();
            Comments.FirstOrDefault(x =>
            {
                if (x.Id == parentId)
                {
                    x.ReplyCommentItems = new ObservableCollection<CommentItem>(replyComments);
                    x.IsInitiateReply = true;
                };
                return x.Id == parentId;
            });
            //IsInitiateReply = true;
            //CommentPresenter.LoadReplyCommentRatingRequest(parentId, comments);
        }

        public void UpdateTotalComments(int count)
        {
            TotalComments = count;
        }

        public void UpdateCommentRating(CommentItemDTO comment)
        {
            var topLevelComment = AutoMapper.AutoMapper.Map<CommentItemDTO, CommentItem>(comment);
            Comments.FirstOrDefault(x =>
            {
                if (x.Id == comment.Id) x.IsLiked = topLevelComment.IsLiked;
                return x.Id == comment.Id;
            });
        }

        //public void UpdateReplyCommentRating(string parentId, CommentItemDTO comment)
        //{
        //    var replyComment = AutoMapper.AutoMapper.Map<CommentItemDTO, CommentItem>(comment);
        //    Comments.FirstOrDefault(x =>
        //    {
        //        if (x.Id == parentId)
        //        {
        //            x.ReplyCommentItems.FirstOrDefault(y =>
        //            {
        //                if (y.Id == comment.Id) y.IsLiked = comment.IsLiked;
        //                return y.Id == comment.Id;
        //            });
        //        };
        //        return x.Id == parentId;
        //    });
        //}
    }
}
