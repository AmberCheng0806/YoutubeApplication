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
        public string CommandText { get; set; }
        public string Id { get; set; }
        public string AuthorImg { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
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
        public ICommand ReplyCommand { get; set; }
        public ICommand CancelReplyCommentsCommand { get; set; }

        public CommentItem(string authorName, string commandText, string id, string authorImg, int likeCount, bool isLiked, DateTime publishedAt, int replyCount, bool isMine)
        {
            AuthorName = authorName;
            CommandText = commandText;
            Id = id;
            AuthorImg = authorImg;
            LikeCount = likeCount;
            IsLiked = isLiked;
            PublishedAt = publishedAt;
            ReplyCount = replyCount;
            IsMyComment = isMine;
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
        }
    }
}
