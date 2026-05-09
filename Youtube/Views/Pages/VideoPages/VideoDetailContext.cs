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
        public string ImageUrl { get; set; } = "https://t3.ftcdn.net/jpg/06/33/54/78/360_F_633547842_AugYzexTpMJ9z1YcpTKUBoqBF0CUCk10.jpg";
        public ObservableCollection<PlaylistItem> Playlists { get; set; } = new ObservableCollection<PlaylistItem>();
        public ObservableCollection<CommentItem> Comments { get; set; } = new ObservableCollection<CommentItem>();
        public bool IsSubscript { get; set; } = false;
        public string RateText { get; set; } = "";
        public string VideoPrivacy { get; set; }
        [DependsOn(nameof(VideoPrivacy))]
        public Visibility CommentsVisibility => VideoPrivacy == "private" ? Visibility.Collapsed : Visibility.Visible;
        [DependsOn(nameof(TotalComments))]
        public Visibility NoCommentsVisibility => TotalComments == 0 ? Visibility.Visible : Visibility.Collapsed;
        [DependsOn(nameof(IsSubscript))]
        public string SubcriptionText => IsSubscript ? "已訂閱" : "訂閱";
        public string CreatePlayListText { get; set; } = "";
        public string PlaylistPrivacyStatus { get; set; } = "public";
        [DependsOn(nameof(CreatePlayListText))]
        public bool IsCreatePlaylistBtnEnable => CreatePlayListText == "" ? false : true;
        public bool IsCreatePlaylistPopup { get; set; } = false;
        public List<OptionsViewModel> Status { get; set; } = new List<OptionsViewModel>() { new OptionsViewModel("public", "公開"), new OptionsViewModel("unlisted", "不公開"), new OptionsViewModel("private", "私人") };
        public int TotalComments { get; set; }
        private YoutubeContext youtubeContext = new YoutubeContext();
        private ICommentPresenter CommentPresenter;
        private IVideoDetailPresenter VideoDetailPresenter;
        public ICommand SubscriptCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand DisLikeCommand { get; set; }
        public ICommand SavePlaylistCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public ICommand AddReplyCommentCommand { get; set; }
        public ICommand ShowReplyCommentsCommand { get; set; }
        public ICommand ClearTopReplyCommentTextCommand { get; set; }
        public ICommand DeleteCommentCommand { get; set; }
        public ICommand EditCommentCommand { get; set; }
        public ICommand CancelCreatePlaylistCommand { get; set; }
        public ICommand CreatePlaylistCommand { get; set; }
        public VideoDetailContext()
        {
            CommentPresenter = new CommentPresenter(this);
            VideoDetailPresenter = new VideoDetailPresenter(this);
        }
        public async void OnNavigatedTo(object[] parameter)
        {

            // textBox.Text = "Hello"
            // Title = "Hello"

            VideoId = parameter[0] as string;
            await VideoDetailPresenter.GetVideoDetailRequest(VideoId);
            await Task.WhenAll(VideoDetailPresenter.GetPlayListRequest(), CommentPresenter.LoadCommentsRequest(VideoId));

            SubscriptCommand = new RelayCommand(async () =>
            {
                IsSubscript = !IsSubscript;
                if (IsSubscript)
                {
                    await VideoDetailPresenter.SubscriptRequest();
                }
                else
                {
                    await VideoDetailPresenter.UnSubscriptRequest();
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
                    VideoDetailPresenter.RemovePlayListRequest(x.PlayListItemVideoId);
                    x.IsAddedVideo = false;
                    new ToastContentBuilder()
                    .AddText("已移除播放清單")
                    .Show();
                }
                else
                {
                    VideoDetailPresenter.SavePlayListRequest(x.Id, VideoId);
                    x.IsAddedVideo = true;
                    new ToastContentBuilder()
                    .AddText("已加入播放清單")
                    .Show();
                }
            });

            //CommentPresenter.LoadCommentsRequest(VideoId);

            AddCommentCommand = new RelayCommand<ReplyCommentContext>(x =>
            {
                CommentPresenter.AddCommentRequest(VideoId, x.CommentText);
                x.CommentText = "";
            });
            WeakReferenceMessenger.Default.Register<CommentItemDTO>(this, (sender, dto) =>
            {
                TotalComments++;
            });
            WeakReferenceMessenger.Default.Register<DeleteCommentDTO, string>(this, "DecreaseCountCommand", (sender, dto) =>
            {
                DeleteCommentDTO comment = (DeleteCommentDTO)dto;
                TotalComments = TotalComments - comment.ReplyCount - 1;
            });
            WeakReferenceMessenger.Default.Register<string, string>(this, "DecreaseReplyCountCommand", (sender, dto) =>
            {
                TotalComments--;
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
            DeleteCommentCommand = new RelayCommand<CommentItem>(x =>
            {
                if (x.ParentId == "")
                {
                    CommentPresenter.DeleteCommentRequest(new DeleteCommentDTO(x.Id, x.ReplyCount, x.ParentId));
                }
                else
                {
                    CommentPresenter.DeleteReplyCommentRequest(new DeleteCommentDTO(x.Id, x.ReplyCount, x.ParentId));
                }
            });
            EditCommentCommand = new RelayCommand<CommentItem>(x =>
            {
                if (x.ParentId == "")
                {
                    CommentPresenter.EditCommentRequest(x.Id, x.EditedCommentText);
                }
                else
                {
                    CommentPresenter.EditReplyCommentRequest(x.ParentId, x.Id, x.EditedCommentText);
                }
                x.CommentText = x.EditedCommentText;
                x.IsEditingMode = false;
            });
            CancelCreatePlaylistCommand = new RelayCommand(() =>
            {
                IsCreatePlaylistPopup = !IsCreatePlaylistPopup;
                if (!IsCreatePlaylistPopup)
                {
                    CreatePlayListText = "";
                    PlaylistPrivacyStatus = "public";
                }
            });
            CreatePlaylistCommand = new RelayCommand(() =>
            {
                VideoDetailPresenter.CreatePlaylistRequest(CreatePlayListText, PlaylistPrivacyStatus, VideoId);
                IsCreatePlaylistPopup = false;
            });
        }


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
            VideoPrivacy = videoDetailDTO.VideoPrivacy;
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

        public void DeleteComment(DeleteCommentDTO deleteCommentDTO)
        {
            var item = Comments.FirstOrDefault(x => x.Id == deleteCommentDTO.CommentId);
            Comments.Remove(item);
            WeakReferenceMessenger.Default.Send(deleteCommentDTO, "DecreaseCountCommand");
        }

        public void DeleteReplyComment(DeleteCommentDTO deleteCommentDTO)
        {
            Comments.FirstOrDefault(x =>
            {
                if (x.Id == deleteCommentDTO.ParentId)
                {
                    var item = x.ReplyCommentItems.FirstOrDefault(y => y.Id == deleteCommentDTO.CommentId);
                    x.ReplyCommentItems.Remove(item);
                    x.ReplyCount--;
                };
                return x.Id == deleteCommentDTO.ParentId;
            });
            WeakReferenceMessenger.Default.Send(deleteCommentDTO.CommentId, "DecreaseReplyCountCommand");
        }

        public void EditComment(string commentId, string commentText)
        {
            var item = Comments.FirstOrDefault(x =>
            {
                if (x.Id == commentId)
                { x.CommentText = commentText; x.IsEdited = true; }
                return x.Id == commentId;
            });
        }

        public void EditReplyComment(string parentId, string commentId, string commentText)
        {
            Comments.FirstOrDefault(x =>
            {
                if (x.Id == parentId)
                {
                    var item = x.ReplyCommentItems.FirstOrDefault(y => y.Id == commentId);
                    item.CommentText = commentText;
                    item.IsEdited = true;
                };
                return x.Id == parentId;
            });
        }

        public void CreatePlaylist(PlaylistItemDTO playlistItemDTO)
        {
            var playlist = AutoMapper.AutoMapper.Map<PlaylistItemDTO, PlaylistItem>(playlistItemDTO);
            Playlists.Add(playlist);
        }
    }
}
