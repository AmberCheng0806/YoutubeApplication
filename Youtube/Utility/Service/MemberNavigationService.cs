using IoC_Container.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Youtube.Views.Pages;

namespace Youtube.Utility.Service
{
    [Singleton("member")]
    public class MemberNavigationService : INavigationService
    {
        public Frame Frame { get; set; }
        private IServiceProvider ServiceProvider { get; set; }
        public Visibility NavigationUIVisibility { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MemberNavigationService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void SetFrame(Frame frame)
        {
            Frame = frame;
            Frame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Page page = e.Content as Page;
            if (page == null) return;
        }

        public void Navigate(string pageKey, params object[] parameter)
        {
            Debug.WriteLine(Frame.GetHashCode());
            Page currentPage = null;
            Type type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name.Contains(pageKey) && x.BaseType == typeof(Page));
            if (type == null) throw new Exception("Page not found");
            currentPage = (Page)ServiceProvider.GetService(type);
            if (currentPage == null) { currentPage = (Page)Activator.CreateInstance(type); }
            if (currentPage.DataContext is INavigationAware aware)
            {
                aware.OnNavigatedTo(parameter);
            }
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
