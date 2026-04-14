using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Youtube.Utility
{
    internal class TextBlockHelper
    {
        public static readonly DependencyProperty HtmlTextProperty =
       DependencyProperty.RegisterAttached(
           "HtmlText",
           typeof(string),
           typeof(TextBlockHelper),
           new PropertyMetadata(null, OnHtmlTextChanged));

        public static string GetHtmlText(DependencyObject obj)
            => (string)obj.GetValue(HtmlTextProperty);

        public static void SetHtmlText(DependencyObject obj, string value)
            => obj.SetValue(HtmlTextProperty, value);

        private static void OnHtmlTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBlock textBlock) return;
            textBlock.Inlines.Clear();
            var text = e.NewValue as string;
            var pattern = @"<a\s+href=""([^""]+)"">([^<]+)</a>";
            var parts = Regex.Split(text, pattern);

            // parts 的結構：[文字, url, linkText, 文字, url, linkText, ...]
            for (int i = 0; i < parts.Length; i++)
            {
                if (i % 3 == 0 && !string.IsNullOrEmpty(parts[i]))
                {
                    var strings = parts[i].Split(new string[] { "<br>" }, StringSplitOptions.None);
                    if (strings.Length == 1)
                    { textBlock.Inlines.Add(new Run(parts[i])); }
                    else
                    {
                        foreach (var item in strings)
                        {
                            textBlock.Inlines.Add(item);
                            textBlock.Inlines.Add(new LineBreak());
                        }
                    }
                }
                else if (i % 3 == 1)
                {
                    var url = parts[i];
                    var linkText = parts[i + 1];
                    i++;

                    var hyperlink = new Hyperlink(new Run(linkText))
                    {
                        NavigateUri = new Uri(url)
                    };
                    hyperlink.RequestNavigate += (s, args) =>
                    {
                        Process.Start(new ProcessStartInfo(args.Uri.AbsoluteUri)
                        {
                            UseShellExecute = true
                        });
                        args.Handled = true;
                    };
                    textBlock.Inlines.Add(hyperlink);
                }
            }
        }
    }
}
