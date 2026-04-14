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
            //void UpdateReplyCommentRating(string parentId, CommentItemDTO commentItemDTOs);
        }

        public interface ICommentPresenter
        {
            void LoadCommentsRequest(string videoId);
            void LoadCommentsRatingRequest(List<CommentItemDTO> commentItemDTOs);
            void AddCommentRequest(string videoId, string commentText);
            void AddReplyCommentRequest(string parentId, string commentText);
            void LoadReplyCommentsRequest(string parentId);
            //void LoadReplyCommentRatingRequest(string parentId, List<CommentItemDTO> commentItemDTOs);
        }
    }
}
