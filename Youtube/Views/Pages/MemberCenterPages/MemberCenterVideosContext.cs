using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Presenters;
using Youtube.Presenters.Models;
using Youtube.Utility.Service;
using YoutubeAPI;
using static Youtube.Contracts.MemberCenterVideosContract;

namespace Youtube.Views.Pages.MemberCenterPages
{
    [AddINotifyPropertyChangedInterface]
    internal class MemberCenterVideosContext : INavigationAware, IMemberCenterVideosView
    {
        public ObservableCollection<MemberCenterVideoModel> Videos { get; set; } = new ObservableCollection<MemberCenterVideoModel>();
        public YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string VideoPrivacyStatus { get; set; }
        public string VideoUrl { get; set; }
        public string SelectVideoBtnText { get; set; } = "選擇影片";
        public List<OptionsViewModel> Status { get; set; } = new List<OptionsViewModel>() { new OptionsViewModel("public", "公開"), new OptionsViewModel("unlisted", "不公開"), new OptionsViewModel("private", "私人") };
        public INavigationService NavigationService { get; set; } = App.NavigationService;
        public ICommand SelectVideoCommand { get; set; }
        public ICommand UploadVideoCommand { get; set; }
        public ICommand DeleteVideoCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ClickVideoCommand { get; set; }
        private IMemberCenterVideosPresenter memberCenterVideosPresenter { get; set; }

        public MemberCenterVideosContext()
        {
            memberCenterVideosPresenter = new MemberCenterVideosPresenter(this);
        }
        public void DeleteVideo(string videoId)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(object[] parameter)
        {
            //var search = await YoutubeContext.Search.GetMyVideos();
            //string videoString = string.Join(",", search.items.Select(x => x.id.videoId).ToArray());
            //var videoDetail = await YoutubeContext.Video.GetByVideoIdAsync(videoString);
            //foreach (var item in videoDetail.items)
            //{
            //    Videos.Add(new MemberCenterVideoModel(item.snippet.thumbnails.medium.url, item.snippet.title, item.snippet.description, item.status.privacyStatus, item.snippet.publishedAt, item.statistics.viewCount, item.statistics.commentCount, item.statistics.likeCount, item.statistics.dislikeCount,
            //        item.id, item.snippet.categoryId, item.snippet.title, item.snippet.description, item.status.privacyStatus));
            //}
            await memberCenterVideosPresenter.GetMyVideosRequest();
            SelectVideoCommand = new RelayCommand(() =>
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog().Value) VideoUrl = dialog.FileName;
                SelectVideoBtnText = dialog.FileName.Split('\\').Last();
            });
            UploadVideoCommand = new RelayCommand(async () =>
            {
                var createVideo = await YoutubeContext.Video.CreateAsync(VideoTitle, VideoUrl, VideoPrivacyStatus);
                Videos.Add(new MemberCenterVideoModel(createVideo.snippet.thumbnails.medium.url, VideoTitle, VideoDescription, VideoPrivacyStatus, createVideo.snippet.publishedAt, "0", "0", "0", "0", createVideo.id, createVideo.snippet.categoryId, createVideo.snippet.title, createVideo.snippet.description, VideoPrivacyStatus));
                VideoTitle = "";
                VideoPrivacyStatus = "public";
                VideoUrl = "";
                VideoDescription = "";
                SelectVideoBtnText = "選擇影片";
            });
            DeleteVideoCommand = new RelayCommand<MemberCenterVideoModel>(async x =>
            {
                await YoutubeContext.Video.DeleteAsync(x.VideoId);
                MemberCenterVideoModel video = Videos.FirstOrDefault(y => y.VideoId == x.VideoId);
                Videos.Remove(video);
            });
            SaveCommand = new RelayCommand<MemberCenterVideoModel>(async x =>
            {
                await YoutubeContext.Video.UpdateAsync(x.VideoId, x.VideoTitle, x.CategoryId, x.VideoDescription, x.PrivacyStatus);
                x.OriginalVideoTitle = x.VideoTitle;
                x.OriginalVideoDescription = x.VideoDescription;
                x.OriginalVideoPrivacy = x.PrivacyStatus;
                x.EditVideoModeVisibility = Visibility.Collapsed;
            });
            CancelCommand = new RelayCommand<MemberCenterVideoModel>(x =>
            {
                x.VideoTitle = x.OriginalVideoTitle;
                x.VideoDescription = x.OriginalVideoDescription;
                x.PrivacyStatus = x.OriginalVideoPrivacy;
                x.EditVideoModeVisibility = Visibility.Collapsed;
            });
            ClickVideoCommand = new RelayCommand<string>(x => NavigationService.Navigate("VideoDetail", x));
        }

        public void RenderVideos(List<MemberCenterVideoDTO> memberCenterVideoDTOs)
        {
            var models = AutoMapper.AutoMapper.Map<MemberCenterVideoDTO, MemberCenterVideoModel>(memberCenterVideoDTOs);
            Videos = new ObservableCollection<MemberCenterVideoModel>(models);
        }

        public void UpdateVideo(MemberCenterVideoDTO memberCenterVideoDTO)
        {
            throw new NotImplementedException();
        }

        public void UploadVideo(MemberCenterVideoDTO memberCenterVideoDTO)
        {
            throw new NotImplementedException();
        }
    }
}
