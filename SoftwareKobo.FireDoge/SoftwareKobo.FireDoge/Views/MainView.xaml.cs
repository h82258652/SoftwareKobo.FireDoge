using SoftwareKobo.FireDoge.Models;
using SoftwareKobo.FireDoge.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SoftwareKobo.FireDoge.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView
    {
        private static bool _isFirstView = true;

        public MainView()
        {
            InitializeComponent();
            if (_isFirstView)
            {
                ViewModel?.Pages.Add(new TabPage());

                _isFirstView = false;
            }
        }

        public MainViewModel ViewModel
        {
            get
            {
                return (MainViewModel)DataContext;
            }
        }

        private void CloseCurrentTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var page = (TabPage)menuItem.DataContext;

            if (ViewModel.Pages.Count == 1 && ViewModel.Pages.ElementAtOrDefault(0) == page)
            {
                // 只有一个标签页，则关闭窗口。
                Close();
            }
            else
            {
                ViewModel.Pages.Remove(page);
            }
        }

        private void CloseOtherTabsMenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            menuItem.IsEnabled = ViewModel.Pages.Count > 1;
        }

        private void CloseOtherTabsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var page = menuItem.DataContext;

            var pages = ViewModel.Pages;
            for (var i = pages.Count - 1; i >= 0; i--)
            {
                var temp = pages[i];
                if (page != temp)
                {
                    pages.Remove(temp);
                }
            }
        }
    }
}