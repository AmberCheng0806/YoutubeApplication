using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Youtube.Presenters;
using Youtube.Presenters.Models;
using Youtube.Utility.Service;
using YoutubeAPI;
using YoutubeAPI.PlayList.Models;
using static Youtube.Contracts.MemberCenterPlaylistsContract;

namespace Youtube.Views.Pages.MemberCenterPages
{
    [AddINotifyPropertyChangedInterface]
    internal class MemberCenterPlaylistsContext : INavigationAware, IMemberCenterPlaylistsView
    {
        public ObservableCollection<MemberCenterPlaylistModel> Playlists { get; set; } = new ObservableCollection<MemberCenterPlaylistModel>();
        public YoutubeContext YoutubeContext { get; set; } = new YoutubeContext();
        public string PlaylistTitle { get; set; } = "";
        public string PlaylistDescription { get; set; } = "";
        public string PlaylistPrivacy { get; set; } = "public";
        public INavigationService NavigationService { get; set; } = App.NavigationService;
        public List<OptionsViewModel> Status { get; set; } = new List<OptionsViewModel>() { new OptionsViewModel("public", "公開"), new OptionsViewModel("unlisted", "不公開"), new OptionsViewModel("private", "私人") };
        public ICommand DeletePlaylistCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteVideoCommamd { get; set; }
        public ICommand ClickVideoCommand { get; set; }
        public ICommand CreatePlaylistCommand { get; set; }
        public ICommand CancelCreatePlaylistCommand { get; set; }
        private IMemberCenterPlaylistsPresenter MemberCenterPlaylistsPresenter { get; set; }

        public MemberCenterPlaylistsContext()
        {
            MemberCenterPlaylistsPresenter = new MemberCenterPlaylistsPresenter(this);
        }

        public async void OnNavigatedTo(object[] parameter)
        {
            await MemberCenterPlaylistsPresenter.GetMyPlaylistsRequest();
            DeletePlaylistCommand = new RelayCommand<MemberCenterPlaylistModel>(async x =>
            {
                await MemberCenterPlaylistsPresenter.DeletePlaylistRequest(x.PlaylistId);
            });
            SaveCommand = new RelayCommand<MemberCenterPlaylistModel>(async x =>
            {
                await MemberCenterPlaylistsPresenter.UpdatePlaylistRequest(x.PlaylistId, x.PlaylistTitle, x.PlaylistDescription, x.PlaylistPrivacyStatus);
                x.OriginalPlaylistTitle = x.PlaylistTitle;
                x.OriginalPlaylistDescription = x.PlaylistDescription;
                x.OriginalPlaylistPrivacy = x.PlaylistPrivacyStatus;
                x.EditPlaylistModeVisibility = Visibility.Collapsed;
            });
            CancelCommand = new RelayCommand<MemberCenterPlaylistModel>(x =>
            {
                x.PlaylistTitle = x.OriginalPlaylistTitle;
                x.PlaylistDescription = x.OriginalPlaylistDescription;
                x.PlaylistPrivacyStatus = x.OriginalPlaylistPrivacy;
                x.EditPlaylistModeVisibility = Visibility.Collapsed;
            });
            CancelCreatePlaylistCommand = new RelayCommand(() =>
            {
                PlaylistTitle = "";
                PlaylistDescription = "";
                PlaylistPrivacy = "public";
            });
            DeleteVideoCommamd = new RelayCommand<PlaylistItemVideo>(async x =>
            {
                await MemberCenterPlaylistsPresenter.DeleteVideoRequest(x.PlaylistVideoId);
            });
            ClickVideoCommand = new RelayCommand<string>(x => NavigationService.Navigate("VideoDetail", x));
            CreatePlaylistCommand = new RelayCommand(async () =>
            {
                if (PlaylistTitle == "") return;
                await MemberCenterPlaylistsPresenter.CreatePlaylistRequest(PlaylistTitle, PlaylistPrivacy, PlaylistDescription);
            });
        }

        public void RenderPlaylists(List<MemberCenterPlaylistDTO> memberCenterPlaylistDTOs)
        {
            var memberCenterPlaylistModels = AutoMapper.AutoMapper.Map<MemberCenterPlaylistDTO, MemberCenterPlaylistModel>(memberCenterPlaylistDTOs);
            Playlists = new ObservableCollection<MemberCenterPlaylistModel>(memberCenterPlaylistModels);
        }

        public void DeletePlaylist(string playlistId)
        {
            MemberCenterPlaylistModel model = Playlists.FirstOrDefault(x => x.PlaylistId == playlistId);
            Playlists.Remove(model);
        }

        public void DeleteVideo(string playlistVideoId)
        {
            Playlists.FirstOrDefault(y =>
            {
                var video = y.PlaylistItemVideos.FirstOrDefault(z => z.PlaylistVideoId == playlistVideoId);
                if (video != null) { y.PlaylistItemVideos.Remove(video); }
                return true;
            });
        }

        public void CreatePlaylist(MemberCenterPlaylistDTO memberCenterPlaylistDTO)
        {
            var model = AutoMapper.AutoMapper.Map<MemberCenterPlaylistDTO, MemberCenterPlaylistModel>(memberCenterPlaylistDTO);
            Playlists.Add(model);
        }
    }
}
