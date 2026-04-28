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
    internal class MemberCenterContext
    {
        private INavigationService MemberNavigationService;
        public ICommand SwitchPageCommand { get; set; }
        public MemberCenterContext(INavigationService navigationService)
        {
            MemberNavigationService = navigationService;
            MemberNavigationService.Navigate("MemberCenterVideos");
            SwitchPageCommand = new RelayCommand<string>(x =>
            {
                MemberNavigationService.Navigate(x);
            });
        }
    }
}
