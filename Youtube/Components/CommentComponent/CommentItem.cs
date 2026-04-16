using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Utility;

namespace Youtube.Components.CommentComponent
{
    [AddINotifyPropertyChangedInterface]
    internal class CommentItem
    {
        public string AuthorName { get; set; }
        public string CommentText { get; set; }
        public string EditedCommentText { get; set; }
        public string Id { get; set; }
        public string AuthorImg { get; set; }
        public string ParentId { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsEditingMode { get; set; } = false;
        public bool IsEdited { get; set; } = false;
        public DateTime PublishedAt { get; set; }
        public bool IsMyComment { get; set; }
        public int ReplyCount { get; set; }
        public bool IsInitiateReply { get; set; } = false;
        public ObservableCollection<CommentItem> ReplyCommentItems { get; set; } = new ObservableCollection<CommentItem>();
        public Visibility ReplyCommentVisibility { get; set; } = Visibility.Collapsed;
        [DependsOn(nameof(IsMyComment))]
        public Visibility EditBtnVisibility => IsMyComment == true ? Visibility.Visible : Visibility.Collapsed;
        [DependsOn(nameof(ReplyCount))]
        public Visibility ReplyCommentCountVisibility => ReplyCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ReplyCommentsCollectionVisibility { get; set; } = Visibility.Collapsed;
        [DependsOn(nameof(IsEditingMode))]
        public Visibility EditingModeVisibility => IsEditingMode == true ? Visibility.Visible : Visibility.Collapsed;
        public ICommand ReplyCommand { get; set; }
        public ICommand CancelReplyCommentsCommand { get; set; }
        public ICommand EditModeCommand { get; set; }
        public ICommand CancelEditModeCommand { get; set; }

        public CommentItem(string authorName, string commentText, string editedText, string id, string authorImg, int likeCount, bool isLiked, DateTime publishedAt, int replyCount, bool isMine, string parentId, bool isEdited)
        {
            AuthorName = authorName;
            CommentText = commentText;
            EditedCommentText = editedText;
            Id = id;
            AuthorImg = authorImg;
            LikeCount = likeCount;
            IsLiked = isLiked;
            PublishedAt = publishedAt;
            ReplyCount = replyCount;
            IsMyComment = isMine;
            ParentId = parentId;
            IsEdited = isEdited;
        }
        public CommentItem()
        {
            ReplyCommand = new RelayCommand(() =>
            {
                if (ReplyCommentVisibility == Visibility.Collapsed)
                {
                    ReplyCommentVisibility = Visibility.Visible;
                }
                else { ReplyCommentVisibility = Visibility.Collapsed; }
            });
            CancelReplyCommentsCommand = new RelayCommand(() =>
            {
                if (ReplyCommentVisibility == Visibility.Collapsed)
                {
                    ReplyCommentVisibility = Visibility.Visible;
                }
                else { ReplyCommentVisibility = Visibility.Collapsed; }
            });
            EditModeCommand = new RelayCommand(() => IsEditingMode = true);
            CancelEditModeCommand = new RelayCommand(() => { IsEditingMode = false; EditedCommentText = CommentText; });
        }
    }
}
