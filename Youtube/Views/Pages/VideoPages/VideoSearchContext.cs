using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Youtube.Contracts.SearchContract;
using System.Windows;
using Youtube.Components.VideoCardComponent;
using Youtube.Presenters.Models;
using PropertyChanged;
using YoutubeAPI.Video;
using Youtube.Presenters;
using Mapper = AutoMapper.AutoMapper;
using System.Windows.Input;
using Youtube.Components.PaginationComponent;
using Youtube.Utility;
using Youtube.Utility.Service;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Shell;
using YoutubeAPI.Video.Models;

namespace Youtube.Views.Pages.VideoPages
{
    [AddINotifyPropertyChangedInterface]
    internal class VideoSearchContext : INavigationAware, ISearchView
    {
        public Visibility LoadingPictureVisibility { get; set; } = Visibility.Collapsed;
        [DependsOn(nameof(VideoCardDTOs))]
        public Visibility PageVisibility => VideoCardDTOs.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        public ObservableCollection<VideoCardContext> VideoCardContexts { get; set; } = new ObservableCollection<VideoCardContext>();
        public List<VideoCardDTO> VideoCardDTOs { get; set; } = new List<VideoCardDTO>();
        [DependsOn(nameof(VideoCardDTOs))]
        public int TotalCount => VideoCardDTOs.Count;

        private ISearchPresenter presenter;
        public ICommand ChangePaginationIndexCommand { get; set; }
        public ICommand ClickVideoCommand { get; set; }

        public VideoSearchContext()
        {
            presenter = new SearchPresenter(this);
            ChangePaginationIndexCommand = new RelayCommand<PaginationDTO>(x =>
            {
                SetCurrentPage(x.PaginationIndex, x.CountPerPage);
            });
            ClickVideoCommand = new RelayCommand<VideoCardContext>((x) =>
            {
                App.NavigationService.Navigate("VideoDetail", x.videoId);
            });
        }

        public void OnNavigatedTo(object[] parameter)
        {
            presenter.SearchRequest((SearchRequestDTO)parameter[0]);
        }

        public void SearchResponse(List<VideoCardDTO> respnose)
        {
            VideoCardDTOs = respnose;
            SetCurrentPage(1, 5);
        }
        private void SetCurrentPage(int PaginationIndex, int CountPerPage)
        {
            int SkipNum = CountPerPage * (PaginationIndex - 1);
            List<VideoCardDTO> videoCardDTOs = VideoCardDTOs.Skip(SkipNum).Take(CountPerPage).ToList();
            List<VideoCardContext> videoCardContexts = Mapper.Map<VideoCardDTO, VideoCardContext>(videoCardDTOs).ToList();
            videoCardContexts.ForEach(context =>
            {
                context.OpenVideoCommand = ClickVideoCommand;
            });
            VideoCardContexts = new ObservableCollection<VideoCardContext>(videoCardContexts);
        }
    }
}
