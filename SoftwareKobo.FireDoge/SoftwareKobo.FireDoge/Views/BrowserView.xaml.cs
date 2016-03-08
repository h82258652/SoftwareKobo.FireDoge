using FontAwesome.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoftwareKobo.FireDoge.Controls;
using SoftwareKobo.FireDoge.Controls.Browsers;
using SoftwareKobo.FireDoge.Datas;
using SoftwareKobo.FireDoge.Models;
using SoftwareKobo.FireDoge.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SoftwareKobo.FireDoge.Views
{
    /// <summary>
    /// BrowserView.xaml 的交互逻辑
    /// </summary>
    public partial class BrowserView : INotifyPropertyChanged
    {
        private CancellationTokenSource _tokenSource;

        public BrowserView()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IBrowserControl Browser
        {
            get
            {
                return (IBrowserControl)BrowserContainer?.Content;
            }
        }

        public WebPage Page
        {
            get
            {
                var tabPage = DataContext as TabPage;
                return tabPage?.Model;
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            new AboutView()
            {
                Owner = Window.GetWindow(this)
            }.ShowDialog();
        }

        private void AddressBar_ItemsSourceRequested(AddressBar addressbar, EventArgs e)
        {
            var query = addressbar.Text;
            if (string.IsNullOrEmpty(query))
            {
                addressbar.ItemsSource = null;
                return;
            }

            if (_tokenSource != null)
            {
                // 取消掉之前的任务。
                _tokenSource.Cancel();
            }

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            Task.Run(async () =>
            {
                try
                {
                    // 1.5 秒后再发起查询吧，要不百毒太可怜了.
                    await Task.Delay(1500, token);
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd(Constants.UserAgent);

                        var response = await client.GetStringAsync("http://suggestion.baidu.com/su?wd=" + query);
                        var match = Regex.Match(response, @"{.*}");
                        if (match.Success)
                        {
                            var json = match.Value;
                            var jobject = JsonConvert.DeserializeObject<JObject>(json);
                            var suggestions = (JArray)jobject["s"];
                            addressbar.Dispatcher.Invoke(() =>
                            {
                                addressbar.ItemsSource = suggestions.Where(temp => temp.Type == JTokenType.String).Select(temp => (string)temp).Take(5);
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }, token);

            //Browser?.NavigateToString("<h1>" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "</h1>");
            //addressbar.ItemsSource = new string[]
            //{
            //    "http://www.baidu.com",
            //    "http://www.cnblogs.com",
            //    Guid.NewGuid().ToString(),
            //    Guid.NewGuid().ToString(),
            //    Guid.NewGuid().ToString(),
            //};
        }

        private void AddressBar_Loaded(object sender, RoutedEventArgs e)
        {
            AddressBar.Focus();
        }

        private void AddressBar_Submited(object sender, AddressBarSubmitedEventArgs e)
        {
            Browser?.Navigate(e.SubmitedAddress);
        }

        private void BrowserContainer_Loaded(object sender, RoutedEventArgs e)
        {
            InitBrowser(AppSetting.DefaultBrowserEngine);
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            var browser = Browser;
            if (browser != null && browser.CanGoBack)
            {
                // 确保能够后退。
                browser.GoBack();
            }
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            var browser = Browser;
            if (browser != null && browser.CanGoForward)
            {
                browser.GoForward();
            }
        }

        private void InitBrowser(BrowserEngine newEngine)
        {
            IBrowserControl browser;
            switch (newEngine)
            {
                case BrowserEngine.Ms:
                    browser = new MsBrowser();
                    SwitchEngineButton.Content = new FontAwesome.WPF.FontAwesome()
                    {
                        Icon = FontAwesomeIcon.InternetExplorer
                    };
                    SwitchEngineButton.ToolTip = "当前为 IE 引擎，按下切换到 Webkit 引擎";
                    break;

                case BrowserEngine.Webkit:
                    browser = new WebkitBrowser();
                    SwitchEngineButton.Content = new FontAwesome.WPF.FontAwesome()
                    {
                        Icon = FontAwesomeIcon.Chrome
                    };
                    SwitchEngineButton.ToolTip = "当前为 Webkit 引擎，按下切换到 IE 引擎";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newEngine), newEngine, null);
            }

            browser.NewWindowRequest += (sender, e) =>
            {
                var rootWindow = Window.GetWindow(this);
                if (rootWindow != null)
                {
                    var mainViewModel = rootWindow.DataContext as MainViewModel;
                    if (mainViewModel != null)
                    {
                        var newPage = new TabPage(e.RequestUrl);
                        mainViewModel.Pages.Add(newPage);
                        mainViewModel.CurrentPage = newPage;
                    }
                }
            };

            BrowserContainer.Content = browser;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Browser)));

            if (Page != null)
            {
                browser.Navigate(Page.Address);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Browser?.Refresh();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            var rootWindow = Window.GetWindow(this);
            SettingView settingView = new SettingView()
            {
                Owner = rootWindow
            };
            settingView.ShowDialog();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Browser?.Stop();
        }

        private void SwitchEngineButton_Click(object sender, RoutedEventArgs e)
        {
            var currentBrowser = BrowserContainer.Content;
            if (currentBrowser == null)
            {
                return;
            }

            if (currentBrowser is MsBrowser)
            {
                InitBrowser(BrowserEngine.Webkit);
            }
            else if (currentBrowser is WebkitBrowser)
            {
                InitBrowser(BrowserEngine.Ms);
            }
        }
    }
}