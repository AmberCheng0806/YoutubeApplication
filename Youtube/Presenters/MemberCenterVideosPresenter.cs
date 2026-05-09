using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;
using YoutubeAPI;
using static Youtube.Contracts.MemberCenterVideosContract;

namespace Youtube.Presenters
{
    internal class MemberCenterVideosPresenter : IMemberCenterVideosPresenter
    {
        private IMemberCenterVideosView memberCenterVideosView { get; set; }
        private YoutubeContext YoutubeContext = new YoutubeContext();
        public MemberCenterVideosPresenter(IMemberCenterVideosView view) { memberCenterVideosView = view; }
        public async Task DeleteVideoRequest(string videoId)
        {
            await YoutubeContext.Video.DeleteAsync(videoId);
            memberCenterVideosView.DeleteVideo(videoId);
        }

        public async Task GetMyVideosRequest()
        {
            var search = await YoutubeContext.Search.GetMyVideos();
            string videoString = string.Join(",", search.items.Select(x => x.id.videoId).ToArray());
            var videoDetail = await YoutubeContext.Video.GetByVideoIdAsync(videoString);
            List<MemberCenterVideoDTO> memberCenterVideoDTOs = new List<MemberCenterVideoDTO>();
            foreach (var item in videoDetail.items)
            {
                memberCenterVideoDTOs.Add(new MemberCenterVideoDTO(item.snippet.thumbnails.medium.url, item.snippet.title, item.snippet.description, item.status.privacyStatus, item.snippet.publishedAt, item.statistics.viewCount, item.statistics.commentCount, item.statistics.likeCount, item.statistics.dislikeCount,
                    item.id, item.snippet.categoryId, item.snippet.title, item.snippet.description, item.status.privacyStatus));
            }
            memberCenterVideosView.RenderVideos(memberCenterVideoDTOs);
        }

        public async Task UpdateVideoRequest(string videoId, string videoTitle, string categoryId, string videoDescription, string privacyStatus)
        {
            await YoutubeContext.Video.UpdateAsync(videoId, videoTitle, categoryId, videoDescription, privacyStatus);
        }

        public async Task UploadVideoRequest(string videoTitle, string videoDescription, string videoUrl, string videoPrivacyStatus)
        {
            var createVideo = await YoutubeContext.Video.CreateAsync(videoTitle, videoDescription, videoUrl, videoPrivacyStatus);
            memberCenterVideosView.UploadVideo(new MemberCenterVideoDTO(createVideo.snippet.thumbnails.medium.url, videoTitle, videoDescription, videoPrivacyStatus, createVideo.snippet.publishedAt, "0", "0", "0", "0", createVideo.id, createVideo.snippet.categoryId, createVideo.snippet.title, createVideo.snippet.description, videoPrivacyStatus));
        }
    }
}
