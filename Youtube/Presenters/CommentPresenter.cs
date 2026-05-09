using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Youtube.Components.CommentComponent;
using Youtube.Presenters.Models;
using YoutubeAPI;
using static Youtube.Contracts.CommentContract;

namespace Youtube.Presenters
{
    internal class CommentPresenter : ICommentPresenter
    {
        public ICommentView CommentView { get; set; }
        private YoutubeContext youtubeContext { get; set; } = new YoutubeContext();
        public CommentPresenter(ICommentView view)
        {
            CommentView = view;
        }

        public async Task LoadCommentsRequest(string videoId)
        {
            var comments = await youtubeContext.Comment.GetCommentByVideoIdAsync(videoId);
            if (comments.items == null) { CommentView.RenderComments(new List<CommentItemDTO>()); return; }
            int total = comments.items.Length;
            foreach (var item in comments.items)
            {
                total += item.snippet.totalReplyCount;
            }
            CommentView.UpdateTotalComments(total);
            var items = comments?.items.Select(item =>
            {
                bool isMine = item.snippet.topLevelComment.snippet.authorChannelId.value == App.ChannelId ? true : false;
                bool isEdited = item.snippet.topLevelComment.snippet.publishedAt == item.snippet.topLevelComment.snippet.updatedAt ? false : true;
                string text = item.snippet.topLevelComment.snippet.textDisplay;
                return new CommentItemDTO(item.snippet.topLevelComment.snippet.authorDisplayName, text, text, item.id, item.snippet.topLevelComment.snippet.authorProfileImageUrl,
                    item.snippet.topLevelComment.snippet.likeCount, false, item.snippet.topLevelComment.snippet.publishedAt, item.snippet.totalReplyCount, new ObservableCollection<CommentItem>(), isMine, "", isEdited);
            }).ToList();
            CommentView.RenderComments(items);
        }

        public async Task AddCommentRequest(string videoId, string commentText)
        {
            var createComment = await youtubeContext.Comment.CreateCommentByVideoIdAsync(videoId, commentText);
            var comment = new CommentItemDTO(createComment.snippet.topLevelComment.snippet.authorDisplayName, commentText, commentText, createComment.id, createComment.snippet.topLevelComment.snippet.authorProfileImageUrl,
                createComment.snippet.topLevelComment.snippet.likeCount, false, createComment.snippet.topLevelComment.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), true, "", false);
            CommentView.AddComment(comment);
        }

        public async Task AddReplyCommentRequest(string parentId, string commentText)
        {
            parentId = parentId.Split('.').FirstOrDefault();
            var createComment = await youtubeContext.Comment.CreateCommentByParentIdAsync(parentId, commentText);
            var comment = new CommentItemDTO(createComment.snippet.authorDisplayName, commentText, commentText, createComment.id, createComment.snippet.authorProfileImageUrl,
                createComment.snippet.likeCount, false, createComment.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), true, parentId, false);
            CommentView.AddReplyComment(parentId, comment);
        }

        public async Task LoadReplyCommentsRequest(string parentId)
        {
            var comment = await youtubeContext.Comment.GetCommentByParentIdAsync(parentId);
            var replyComments = comment.items.Select(x =>
            {
                bool isMine = x.snippet.authorChannelId.value == App.ChannelId ? true : false;
                bool isEdited = x.snippet.publishedAt == x.snippet.updatedAt ? false : true;
                string text = x.snippet.textDisplay;
                bool isLiked = x.snippet.viewerRating == "none" ? false : true;
                return new CommentItemDTO(x.snippet.authorDisplayName, text, text, x.id, x.snippet.authorProfileImageUrl,
               x.snippet.likeCount, isLiked, x.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), isMine, parentId, isEdited);
            }).ToList();
            CommentView.RenderReplyComments(parentId, replyComments);
        }

        public void LoadCommentsRatingRequest(List<CommentItemDTO> commentItemDTOs)
        {
            var dtos = commentItemDTOs.Select(async item =>
             {
                 var commentDetail = await youtubeContext.Comment.GetCommentByCommentIdAsync(item.Id);
                 bool isLiked = commentDetail.items[0].snippet.topLevelComment.snippet.viewerRating == "none" ? false : true;
                 item.IsLiked = isLiked;
                 CommentView.UpdateCommentRating(item);
             }).ToList();
            //foreach (var item in commentItemDTOs)
            //{
            //    var commentDetail = await youtubeContext.Comment.GetCommentByCommentIdAsync(item.Id);
            //    bool isLiked = commentDetail.items[0].snippet.topLevelComment.snippet.viewerRating == "none" ? false : true;
            //    item.IsLiked = isLiked;
            //}

            //Task.Run(async () =>
            //{
            //    await Task.WhenAll(dtos);
            //    CommentView.UpdateCommentsRating(commentItemDTOs);
            //});


        }

        public async Task DeleteCommentRequest(DeleteCommentDTO deleteCommentDTO)
        {
            var deleteComment = await youtubeContext.Comment.DeleteAsync(deleteCommentDTO.CommentId);
            CommentView.DeleteComment(deleteCommentDTO);
        }

        public async Task DeleteReplyCommentRequest(DeleteCommentDTO deleteCommentDTO)
        {
            var deleteComment = await youtubeContext.Comment.DeleteAsync(deleteCommentDTO.CommentId);
            CommentView.DeleteReplyComment(deleteCommentDTO);
        }

        public async Task EditCommentRequest(string commentId, string commentText)
        {
            var updateComment = await youtubeContext.Comment.UpdateCommentByCommentIdAsync(commentId, commentText);
            var comment = new CommentItemDTO(updateComment.snippet.authorDisplayName, commentText, commentText, updateComment.id, updateComment.snippet.authorProfileImageUrl,
            updateComment.snippet.likeCount, false, updateComment.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), true, "", true);
            CommentView.EditComment(commentId, commentText);
            //CommentView.DeleteComment(new DeleteCommentDTO(commentId,updateComment.snippet.));
            //CommentView.AddComment(comment);
        }

        public async Task EditReplyCommentRequest(string parentId, string commentId, string commentText)
        {
            parentId = parentId.Split('.').FirstOrDefault();
            var updateComment = await youtubeContext.Comment.UpdateCommentByCommentIdAsync(commentId, commentText);
            var comment = new CommentItemDTO(updateComment.snippet.authorDisplayName, commentText, commentText, updateComment.id, updateComment.snippet.authorProfileImageUrl,
            updateComment.snippet.likeCount, false, updateComment.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), true, parentId, true);
            CommentView.EditReplyComment(parentId, commentId, commentText);
            //CommentView.DeleteReplyComment(parentId, commentId);
            //CommentView.AddReplyComment(parentId, comment);
        }

    }
}
