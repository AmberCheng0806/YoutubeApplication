using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Components.CommentComponent;
using Youtube.Presenters.Models;

namespace Youtube.Contracts
{
    internal class CommentContract
    {
        public interface ICommentView
        {
            void RenderComments(List<CommentItemDTO> comments);
            void AddComment(CommentItemDTO comment);
            void AddReplyComment(string parentId, CommentItemDTO comment);
            void RenderReplyComments(string parentId, List<CommentItemDTO> comments);
            void UpdateTotalComments(int count);
            void UpdateCommentRating(CommentItemDTO commentItemDTOs);
            void DeleteComment(DeleteCommentDTO deleteCommentDTO);
            void DeleteReplyComment(DeleteCommentDTO deleteCommentDTO);
            void EditComment(string commentId, string commentText);
            void EditReplyComment(string parentId, string commentId, string commentText);
        }

        public interface ICommentPresenter
        {
            Task LoadCommentsRequest(string videoId);
            void LoadCommentsRatingRequest(List<CommentItemDTO> commentItemDTOs);
            Task AddCommentRequest(string videoId, string commentText);
            Task AddReplyCommentRequest(string parentId, string commentText);
            Task LoadReplyCommentsRequest(string parentId);
            Task DeleteCommentRequest(DeleteCommentDTO deleteCommentDTO);
            Task DeleteReplyCommentRequest(DeleteCommentDTO deleteCommentDTO);
            Task EditCommentRequest(string commentId, string commentText);
            Task EditReplyCommentRequest(string parentId, string commentId, string commentText);
        }
    }
}
