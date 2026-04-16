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
        public string CommentText { get; set; }
        public string EditedCommentText { get; set; }
        public string Id { get; set; }
        public string AuthorImg { get; set; }
        public string ParentId { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsEdited { get; set; } = false;
        public DateTime PublishedAt { get; set; }
        public bool IsMyComment { get; set; }
        public int ReplyCount { get; set; }
        public ObservableCollection<CommentItem> ReplyCommentItems { get; set; } = new ObservableCollection<CommentItem>();

        public CommentItemDTO(string authorName, string commentText, string editedText, string id, string authorImg, int likeCount, bool isLiked, DateTime publishedAt, int replyCount, ObservableCollection<CommentItem> commentItems, bool isMine, string parentId, bool isEdited)
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
            ReplyCommentItems = commentItems;
            IsMyComment = isMine;
            ParentId = parentId;
            IsEdited = isEdited;
        }
        public CommentItemDTO() { }
    }
}
