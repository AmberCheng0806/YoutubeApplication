
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Youtube.Views;
using Youtube.Views.Pages.VideoPages;

namespace Youtube.Utility
{
    internal static class WebView2Helper
    {
        public static HttpListener Listener = null;
        public static void SetVideoID(DependencyObject obj, string value)
        {
            obj.SetValue(VideoIDProperty, value);
        }

        public static string GetVideoID(DependencyObject obj)
        {
            return (string)obj.GetValue(VideoIDProperty);
        }

        public static readonly DependencyProperty VideoIDProperty =
            DependencyProperty.RegisterAttached(
                "VideoID",
                typeof(String),
                typeof(WebView2Helper),
                new PropertyMetadata(OnPrpertyChanged));


        public static async void OnPrpertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView2 webView2 = (WebView2)d;
            await (webView2.EnsureCoreWebView2Async(null));
            if (Listener == null)
            {
                Listener = new HttpListener();
                string url = "http://localhost:5000/";
                Listener.Prefixes.Add(url);
                Listener.Start();
                webView2.CoreWebView2.Navigate("http://localhost:5000/");
                HttpListenerContext context = await Listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;
                string VideoID = e.NewValue.ToString();
                string responseString = $"<!doctype html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <title>Document</title>\r\n  </head>\r\n  <body>\r\n    <iframe\r\n      width=\"975\"\r\n      height=\"548\"\r\n      src=\"https://www.youtube.com/embed/{VideoID}\"\r\n      referrerpolicy=\"strict-origin-when-cross-origin\"\r\n      allowfullscreen\r\n    ></iframe>\r\n  </body>\r\n</html>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
                Listener.Stop();
                Listener = null;
            }
        }
    }

}
