using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube.Presenters.Models;

namespace Youtube.Contracts
{
    internal class MemberCenterVideosContract
    {
        public interface IMemberCenterVideosView
        {
            void RenderVideos(List<MemberCenterVideoDTO> memberCenterVideoDTOs);
            void DeleteVideo(string videoId);
            void UploadVideo(MemberCenterVideoDTO memberCenterVideoDTO);
        }

        public interface IMemberCenterVideosPresenter
        {
            Task GetMyVideosRequest();
            Task DeleteVideoRequest(string videoId);
            Task UpdateVideoRequest(string videoId, string videoTitle, string categoryId, string videoDescription, string privacyStatus);
            Task UploadVideoRequest(string videoTitle, string videoDescription, string videoUrl, string videoPrivacyStatus);
        }
    }
}
