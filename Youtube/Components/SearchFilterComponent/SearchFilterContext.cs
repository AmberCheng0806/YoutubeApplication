using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Presenters.Enums;
using Youtube.Presenters.Models;
using Youtube.Utility;

namespace Youtube.Components.SearchFilterComponent
{
    [AddINotifyPropertyChangedInterface]
    internal class SearchFilterContext
    {
        public ObservableCollection<OptionsViewModel> Types { get; set; }
        public ObservableCollection<OptionsViewModel> PublishedDates { get; set; }
        public ObservableCollection<OptionsViewModel> Durations { get; set; }
        public ObservableCollection<OptionsViewModel> Categories { get; set; }

        public ICommand CheckCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public Visibility FilterVisibility { get; set; }


        public SearchFilterContext(SearchFilter searchFilter)
        {
            Types = new ObservableCollection<OptionsViewModel>()
            {
                new OptionsViewModel("video", "影片"),
                new OptionsViewModel("channel", "頻道"),
                new OptionsViewModel("playlist", "播放清單")
            };

            Durations = new ObservableCollection<OptionsViewModel>()
            {
                new OptionsViewModel("short", "3 分鐘內"),
                new OptionsViewModel("medium", "3 到 20 分鐘"),
                new OptionsViewModel("long", "超過 20 分鐘"),
            };

            PublishedDates = new ObservableCollection<OptionsViewModel>()
            {
                new OptionsViewModel(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), "今天"),
                new OptionsViewModel(DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddTHH:mm:ssZ"), "本週"),
                new OptionsViewModel(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"), "本月"),
                new OptionsViewModel(DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"), "今年"),
            };

            VideoType[] values = (VideoType[])Enum.GetValues(typeof(VideoType));
            Categories = new ObservableCollection<OptionsViewModel>
            (
               values.Select(x => new OptionsViewModel(x.ToString(), x.ToString()))
            );

            CheckCommand = new RelayCommand(() =>
            {
                FilterVisibility = Types.Any(x => x.Key == "video" && x.IsChecked) ? Visibility.Visible : Visibility.Collapsed;
            });

            ApplyCommand = new RelayCommand(() =>
            {
                string type = string.Join(", ", Types.Where(x => x.IsChecked).Select(x => x.Key));
                DateTime publishedAfter = PublishedDates.Where(x => x.IsChecked).Select(x => DateTime.Parse(x.Key)).FirstOrDefault();
                string videoDuration = Durations.Where(x => x.IsChecked).Select(x => x.Key).FirstOrDefault();
                VideoType videoCategoryId = Categories.Where(x => x.IsChecked).Select(x => (VideoType)Enum.Parse(typeof(VideoType), x.Key)).FirstOrDefault();
                var dto = new SearchFilterDTO(type, publishedAfter, videoDuration, videoCategoryId);
                searchFilter.Execute(dto);
            });
        }
    }
}
