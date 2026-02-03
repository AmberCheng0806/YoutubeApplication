using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Components.SearchFilterComponent;
using Youtube.Components.VideoCardComponent;
using Youtube.Presenters;
using Youtube.Presenters.Enums;
using Youtube.Presenters.Models;
using Youtube.Utility;
using static Youtube.Contracts.SearchContract;
using Mapper = AutoMapper.AutoMapper;
namespace Youtube.Views
{
    [AddINotifyPropertyChangedInterface]
    internal class MainWindowContext : ISearchView
    {
        public string SearchText { get; set; }
        public ObservableCollection<VideoCardContext> VideoCardContexts { get; set; } = new ObservableCollection<VideoCardContext>();

        private ISearchPresenter presenter;
        public ICommand SearchCommand { get; set; }

        public ICommand SearchConditionCommand { get; set; }
        public SearchFilterDTO SearchFilter { get; set; } = new SearchFilterDTO();

        public MainWindowContext()
        {
            presenter = new SearchPresenter(this);
            SearchConditionCommand = new RelayCommand<SearchFilterDTO>(x => this.SearchFilter = x);
            SearchCommand = new RelayCommand<string>(x => presenter.SearchRequest(GetSearchRequest()));
        }
        public void SearchResponse(List<VideoCardDTO> respnose)
        {
            var videoCardContexts = Mapper.Map<VideoCardDTO, VideoCardContext>(respnose);
            this.VideoCardContexts = new ObservableCollection<VideoCardContext>(videoCardContexts);
        }

        private SearchRequestDTO GetSearchRequest()
        {
            return new SearchRequestDTO(SearchText, SearchFilter.Type, SearchFilter.PublishedAfter, SearchFilter.Duration, SearchFilter.VideoType);
        }
    }
}
