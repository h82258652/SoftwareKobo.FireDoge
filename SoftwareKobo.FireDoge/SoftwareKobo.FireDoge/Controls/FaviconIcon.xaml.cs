using SoftwareKobo.FireDoge.Models;
using SoftwareKobo.FireDoge.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SoftwareKobo.FireDoge.Controls
{
    /// <summary>
    /// FaviconIcon.xaml 的交互逻辑
    /// </summary>
    public partial class FaviconIcon
    {
        public FaviconIcon()
        {
            InitializeComponent();
        }

        public WebPage Page
        {
            get
            {
                var tabPage = DataContext as TabPage;
                return tabPage?.Model;
            }
        }

        private void FaviconIcon_Loaded(object sender, RoutedEventArgs e)
        {
            var page = Page;
            if (page != null)
            {
                // 监听属性发生变化。
                page.PropertyChanged += Page_PropertyChanged;
            }
        }

        private async void Page_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WebPage.Favicon))
            {
                // 变化的属性是 Favicon。
                if (Page != null)
                {
                    // 下载或从缓存读取 Favicon 并显示。
                    var cacheFilePath = await WebFileCache.GetFilePathAsync(Page.Favicon);
                    if (cacheFilePath != null)
                    {
                        try
                        {
                            FaviconImage.Source = new BitmapImage(new Uri(cacheFilePath));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}