using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Youtube.Components.PaginationComponent;
using Youtube.Views.Pages;
using Page = System.Windows.Controls.Page;

namespace Youtube.Utility.Service
{
    [AddINotifyPropertyChangedInterface]
    internal class NavigationService : INavigationService
    {
        public Frame Frame { get; set; }
        public Visibility NavigationUIVisibility { get; set; } = Visibility.Collapsed;
        public NavigationService(Frame frame)
        {
            Frame = frame;
            Frame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Page page = e.Content as Page;
            if (page == null) return;
            if (page.Title.Contains("VideoDetail") || page.Title.Contains("VideoSearch")) { NavigationUIVisibility = Visibility.Visible; }
            else { NavigationUIVisibility = Visibility.Collapsed; };
        }

        public void Navigate(string pageKey, params object[] parameter)
        {
            Page currentPage = null;
            Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name.Contains(pageKey) && x.BaseType == typeof(Page));
            if (type == null) throw new Exception("Page not found");
            currentPage = (Page)Activator.CreateInstance(type);
            if (currentPage.DataContext is INavigationAware aware)
            {
                aware.OnNavigatedTo(parameter);
            }
            if (pageKey == "VideoDetail" || pageKey == "VideoSearch") { NavigationUIVisibility = Visibility.Visible; }
            else { NavigationUIVisibility = Visibility.Collapsed; };
            Frame.Navigate(currentPage);
        }


        public void GoBack()
        {
            if (Frame.NavigationService.CanGoBack)
                Frame.NavigationService.GoBack();
        }

        public void GoForward()
        {
            if (Frame.NavigationService.CanGoForward)
                Frame.NavigationService.GoForward();
        }

        public void GoHome()
        {
            Frame.Content = null;
        }
    }
}
