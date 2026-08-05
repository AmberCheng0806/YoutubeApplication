using IoC_Container.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube.Utility;
using Youtube.Utility.Service;

namespace Youtube.Views.Pages.MemberCenterPages
{
    [Singleton]
    public class MemberCenterContext : INavigationAware
    {
        public INavigationService MemberNavigationService;
        public ICommand SwitchPageCommand { get; set; }
        public MemberCenterContext([GetInstance("member")] INavigationService navigationService)
        {
            MemberNavigationService = navigationService;
            SwitchPageCommand = new RelayCommand<string>(x =>
            {
                MemberNavigationService.Navigate(x);
            });
        }

        public void OnNavigatedTo(object[] parameter)
        {
            MemberNavigationService.Navigate("MemberCenterVideos");
        }
    }
}
