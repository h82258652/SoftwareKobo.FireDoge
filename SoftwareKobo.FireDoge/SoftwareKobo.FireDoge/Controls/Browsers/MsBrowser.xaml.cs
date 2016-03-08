using Microsoft.Win32;
using SoftwareKobo.FireDoge.Models;
using SoftwareKobo.FireDoge.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    /// <summary>
    /// MsBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class MsBrowser : INotifyPropertyChanged, IBrowserControl
    {
        private readonly WebBrowserEx _hostBrowser = new WebBrowserEx()
        {
            // 禁止显示 JS 错误。
            ScriptErrorsSuppressed = true
        };

        private string _lastAddress;

        static MsBrowser()
        {
            #region 不使用兼容模式。

            // 获取当前应用程序的名称。
            var applicationName = Constants.AppNameWithExtension;

            // https://support.askia.com/hc/en-us/articles/200011472-Change-the-version-of-Internet-Explorer-in-askiadesign-s-web-test-mode
            if (Environment.Is64BitOperatingSystem == false)
            {
                // 32 bit.
                Registry.LocalMachine?.OpenSubKey("SOFTWARE")?.OpenSubKey("Microsoft")?.OpenSubKey("Internet Explorer")?.OpenSubKey("MAIN")?.OpenSubKey("FeatureControl")?.OpenSubKey("FEATURE_BROWSER_EMULATION", true)?.SetValue(applicationName, 0x2af8);
            }
            else
            {
                // 64 bit.
                Registry.LocalMachine?.OpenSubKey("Software")?.OpenSubKey("wow6432node")?.OpenSubKey("Microsoft")?.OpenSubKey("Internet Explorer")?.OpenSubKey("Main")?.OpenSubKey("FeatureControl")?.OpenSubKey("FEATURE_BROWSER_EMULATION", true)?.SetValue(applicationName, 0x2af8);
            }

            #endregion 不使用兼容模式。
        }

        public MsBrowser()
        {
            InitializeComponent();
            Init();
        }

        public event AddressChangedEventHandler AddressChanged;

        public event NewWindowRequestEventHandler NewWindowRequest;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Address
        {
            get
            {
                return _hostBrowser.Url?.AbsoluteUri;
            }
        }

        public bool CanGoBack
        {
            get
            {
                return _hostBrowser.CanGoBack;
            }
        }

        public bool CanGoForward
        {
            get
            {
                return _hostBrowser.CanGoForward;
            }
        }

        public bool IsLoading
        {
            get
            {
                return _hostBrowser.IsBusy;
            }
        }

        public WebPage Page
        {
            get
            {
                return (WebPage)DataContext;
            }
        }

        public void GoBack()
        {
            _hostBrowser.GoBack();
        }

        public void GoForward()
        {
            _hostBrowser.GoForward();
        }

        public Task<string> InvokeScriptAsync(string scriptName, params object[] args)
        {
            object result;
            try
            {
                result = _hostBrowser?.Document?.InvokeScript(scriptName, args);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                return Task.FromResult<string>(null);
            }
            return Task.FromResult(result?.ToString());
        }

        public void Navigate(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            _hostBrowser.Navigate(UrlConvert.Convert(url));
        }

        public void NavigateToString(string html)
        {
            _hostBrowser.DocumentText = html ?? string.Empty;
        }

        public void Refresh()
        {
            _hostBrowser.Refresh();
        }

        public void Stop()
        {
            _hostBrowser.Stop();
        }

        private void Init()
        {
            Host.Child = _hostBrowser;
            _hostBrowser.CanGoBackChanged += delegate
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoBack)));
            };
            _hostBrowser.CanGoForwardChanged += delegate
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoForward)));
            };
            _hostBrowser.StatusTextChanged += delegate
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));

                var temp = _hostBrowser.Url?.AbsolutePath;// 获取当前 Url。
                if (_lastAddress != temp)
                {
                    // Url 发生了变化。
                    _lastAddress = temp;
                    AddressChanged?.Invoke(this, new AddressChangedEventArgs(Address));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
                }
            };
            _hostBrowser.DocumentTitleChanged += delegate
            {
                if (Page != null)
                {
                    Page.Title = _hostBrowser.DocumentTitle;
                }
            };
            _hostBrowser.Navigated += async (sender, e) =>
            {
                var page = Page;
                if (page != null)
                {
                    var favicon = (await Favicon.GetFaviconAsync(e.Url))?.AbsoluteUri;
                    page.Favicon = favicon;
                }
            };
            _hostBrowser.BeforeNewWindow += (sender, e) =>
            {
                var eEx = e as WebBrowserExNavigatingEventArgs;
                if (eEx != null)
                {
                    NewWindowRequest?.Invoke(this, new NewWindowRequestEventArgs(eEx.Url));
                    eEx.Cancel = true;
                }
            };
        }
    }
}