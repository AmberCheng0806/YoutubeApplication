using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public async void LoadCommentsRequest(string videoId)
        {
            var comments = await youtubeContext.Comment.GetCommentByVideoIdAsync(videoId);
            int total = comments.items.Length;
            foreach (var item in comments.items)
            {
                total += item.snippet.totalReplyCount;
            }
            CommentView.UpdateTotalComments(total);
            var items = comments?.items.Select(item =>
            {
                bool isMine = item.snippet.topLevelComment.snippet.authorChannelId.value == App.ChannelId ? true : false;
                return new CommentItemDTO(item.snippet.topLevelComment.snippet.authorDisplayName, item.snippet.topLevelComment.snippet.textDisplay, item.id, item.snippet.topLevelComment.snippet.authorProfileImageUrl,
                    item.snippet.topLevelComment.snippet.likeCount, false, item.snippet.topLevelComment.snippet.publishedAt, item.snippet.totalReplyCount, new ObservableCollection<CommentItem>(), isMine);
            }).ToList();
            CommentView.RenderComments(items);
        }

        public async void AddCommentRequest(string videoId, string commentText)
        {
            var createComment = await youtubeContext.Comment.CreateCommentByVideoIdAsync(videoId, commentText);
            var comment = new CommentItemDTO(createComment.snippet.topLevelComment.snippet.authorDisplayName, createComment.snippet.topLevelComment.snippet.textDisplay, createComment.id, createComment.snippet.topLevelComment.snippet.authorProfileImageUrl,
                createComment.snippet.topLevelComment.snippet.likeCount, false, createComment.snippet.topLevelComment.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), true);
            CommentView.AddComment(comment);
        }

        public async void AddReplyCommentRequest(string parentId, string commentText)
        {
            parentId = parentId.Split('.').FirstOrDefault();
            var createComment = await youtubeContext.Comment.CreateCommentByParentIdAsync(parentId, commentText);
            var comment = new CommentItemDTO(createComment.snippet.authorDisplayName, createComment.snippet.textDisplay, createComment.id, createComment.snippet.authorProfileImageUrl,
                createComment.snippet.likeCount, false, createComment.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), true);
            CommentView.AddReplyComment(parentId, comment);
        }

        public async void LoadReplyCommentsRequest(string parentId)
        {
            var comment = await youtubeContext.Comment.GetCommentByParentIdAsync(parentId);
            var replyComments = comment.items.Select(x =>
            {
                bool isMine = x.snippet.authorChannelId.value == App.ChannelId ? true : false;
                //var commentDetail = await youtubeContext.Comment.GetCommentByCommentIdAsync(x.id);
                bool isLiked = x.snippet.viewerRating == "none" ? false : true;
                return new CommentItemDTO(x.snippet.authorDisplayName, x.snippet.textDisplay, x.id, x.snippet.authorProfileImageUrl,
               x.snippet.likeCount, isLiked, x.snippet.publishedAt, 0, new ObservableCollection<CommentItem>(), isMine);
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

        //public void LoadReplyCommentRatingRequest(string parentId, List<CommentItemDTO> commentItemDTOs)
        //{
        //    //foreach (var item in commentItemDTOs)
        //    //{
        //    //    var commentDetail = await youtubeContext.Comment.GetCommentByCommentIdAsync(item.Id);
        //    //    bool isLiked = commentDetail.items[0].snippet.topLevelComment.snippet.viewerRating == "none" ? false : true;
        //    //    item.IsLiked = isLiked;
        //    //}
        //    //CommentView.UpdateReplyCommentsRating(parentId, commentItemDTOs);

        //    var dtos = commentItemDTOs.Select(async item =>
        //    {
        //        var commentDetail = await youtubeContext.Comment.GetCommentByCommentIdAsync(item.Id);
        //        bool isLiked = commentDetail.items[0].snippet.topLevelComment.snippet.viewerRating == "none" ? false : true;
        //        item.IsLiked = isLiked;
        //        CommentView.UpdateReplyCommentRating(parentId, item);
        //    }).ToList();
        //}
    }
}
