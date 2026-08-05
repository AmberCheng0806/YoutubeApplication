using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Youtube.Utility.Service
{
    public interface INavigationService
    {
        void Navigate(string pageKey, params object[] parameter);
        void GoBack();
        void GoForward();
        void GoHome();
        Visibility NavigationUIVisibility { get; set; }
        void SetFrame(Frame frame);
    }
}
