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
using YoutubeAPI.Video.Models;
using static Youtube.Contracts.MemberCenterVideosContract;

namespace Youtube.Views.Pages.MemberCenterPages
{
    [AddINotifyPropertyChangedInterface]
    internal class MemberCenterVideosContext : INavigationAware, IMemberCenterVideosView
    {
        public ObservableCollection<MemberCenterVideoModel> Videos { get; set; } = new ObservableCollection<MemberCenterVideoModel>();
        public YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        public string VideoTitle { get; set; } = "";
        public string VideoDescription { get; set; } = "";
        public string VideoPrivacyStatus { get; set; } = "public";
        public string VideoUrl { get; set; } = "";
        public string SelectVideoBtnText { get; set; } = "選擇影片";
        public List<OptionsViewModel> Status { get; set; } = new List<OptionsViewModel>() { new OptionsViewModel("public", "公開"), new OptionsViewModel("unlisted", "不公開"), new OptionsViewModel("private", "私人") };
        public INavigationService NavigationService { get; set; } = App.NavigationService;
        public ICommand SelectVideoCommand { get; set; }
        public ICommand UploadVideoCommand { get; set; }
        public ICommand DeleteVideoCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ClickVideoCommand { get; set; }
        public ICommand CancelUploadVideosCommand { get; set; }
        private IMemberCenterVideosPresenter memberCenterVideosPresenter { get; set; }

        public MemberCenterVideosContext()
        {
            memberCenterVideosPresenter = new MemberCenterVideosPresenter(this);
        }

        public async void OnNavigatedTo(object[] parameter)
        {
            await memberCenterVideosPresenter.GetMyVideosRequest();
            SelectVideoCommand = new RelayCommand(() =>
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog().Value) VideoUrl = dialog.FileName;
                SelectVideoBtnText = dialog.FileName.Split('\\').Last();
            });
            UploadVideoCommand = new RelayCommand(async () =>
            {
                if (VideoTitle == "" || VideoUrl == "") return;
                await memberCenterVideosPresenter.UploadVideoRequest(VideoTitle, VideoDescription, VideoUrl, VideoPrivacyStatus);
            });
            DeleteVideoCommand = new RelayCommand<MemberCenterVideoModel>(async x =>
            {
                await memberCenterVideosPresenter.DeleteVideoRequest(x.VideoId);
            });
            SaveCommand = new RelayCommand<MemberCenterVideoModel>(async x =>
            {
                await memberCenterVideosPresenter.UpdateVideoRequest(x.VideoId, x.VideoTitle, x.CategoryId, x.VideoDescription, x.PrivacyStatus);
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
            CancelUploadVideosCommand = new RelayCommand(() =>
            {
                VideoTitle = "";
                VideoDescription = "";
                VideoPrivacyStatus = "public";
                SelectVideoBtnText = "選擇影片";
                VideoUrl = "";
            });
        }

        public void RenderVideos(List<MemberCenterVideoDTO> memberCenterVideoDTOs)
        {
            var models = AutoMapper.AutoMapper.Map<MemberCenterVideoDTO, MemberCenterVideoModel>(memberCenterVideoDTOs);
            Videos = new ObservableCollection<MemberCenterVideoModel>(models);
        }

        public void UploadVideo(MemberCenterVideoDTO memberCenterVideoDTO)
        {
            MemberCenterVideoModel member = AutoMapper.AutoMapper.Map<MemberCenterVideoDTO, MemberCenterVideoModel>(memberCenterVideoDTO);
            Videos.Add(member);
            VideoTitle = "";
            VideoPrivacyStatus = "public";
            VideoUrl = "";
            VideoDescription = "";
            SelectVideoBtnText = "選擇影片";
        }
        public void DeleteVideo(string videoId)
        {
            MemberCenterVideoModel video = Videos.FirstOrDefault(y => y.VideoId == videoId);
            Videos.Remove(video);
        }
    }
}
