using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Youtube.Components.PaginationComponent;
using Youtube.Components.SearchFilterComponent;
using Youtube.Components.VideoCardComponent;
using Youtube.Presenters;
using Youtube.Presenters.Enums;
using Youtube.Presenters.Models;
using Youtube.Utility;
using Youtube.Utility.Service;
using static Youtube.Contracts.SearchContract;
using Mapper = AutoMapper.AutoMapper;
namespace Youtube.Views
{
    [AddINotifyPropertyChangedInterface]
    internal class MainWindowContext
    {
        public string Title { get; set; } = "Hello World";
        public string SearchText { get; set; }
        public string MyChannelImgUrl { get; set; } = App.ChannelImg;
        public bool IsFilterConditionPopup { get; set; } = false;
        public ICommand SearchCommand { get; set; }
        public ICommand SearchConditionCommand { get; set; }
        public ICommand ClickMyChannelImgCommand { get; set; }
        public SearchFilterDTO SearchFilter { get; set; } = new SearchFilterDTO();

        public INavigationService NavigationService { get; set; }

        public MainWindowContext(INavigationService navigationService)
        {
            NavigationService = navigationService;
            //presenter = new SearchPresenter(this);
            SearchConditionCommand = new RelayCommand<SearchFilterDTO>(x => { this.SearchFilter = x; IsFilterConditionPopup = false; });
            SearchCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("VideoSearch", CreateSearchRequest());
            });
            ClickMyChannelImgCommand = new RelayCommand(() =>
            {
                navigationService.Navigate("MemberCenter", null);
            });
        }

        private SearchRequestDTO CreateSearchRequest()
        {
            return new SearchRequestDTO(SearchText, SearchFilter.Type, SearchFilter.PublishedAfter, SearchFilter.Duration, SearchFilter.VideoType);
        }
    }
}
