using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Components.CommentComponent;

namespace Youtube.Presenters.Models
{
    internal class CommentItemDTO
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
        public ObservableCollection<CommentItem> ReplyCommentItems { get; set; } = new ObservableCollection<CommentItem>();

        public CommentItemDTO(string authorName, string commandText, string id, string authorImg, int likeCount, bool isLiked, DateTime publishedAt, int replyCount, ObservableCollection<CommentItem> commentItems, bool isMine)
        {
            AuthorName = authorName;
            CommandText = commandText;
            Id = id;
            AuthorImg = authorImg;
            LikeCount = likeCount;
            IsLiked = isLiked;
            PublishedAt = publishedAt;
            ReplyCount = replyCount;
            ReplyCommentItems = commentItems;
            IsMyComment = isMine;
        }
        public CommentItemDTO() { }
    }
}
