using CefSharp;
using CefSharp.WinForms;
using GalaSoft.MvvmLight;
using SoftwareKobo.FireDoge.Models;
using SoftwareKobo.FireDoge.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    /// <summary>
    /// WebkitBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class WebkitBrowser : INotifyPropertyChanged, IBrowserControl
    {
        private ChromiumWebBrowser _hostBrowser;

        public WebkitBrowser()
        {
            InitializeComponent();
            Init();
        }

        public event AddressChangedEventHandler AddressChanged;

        public event NewWindowRequestEventHandler NewWindowRequest;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TaskCompletionSource<object> _initTcs = new TaskCompletionSource<object>();

        private string _address;

        public string Address
        {
            get
            {
                return _address;
            }
            private set
            {
                if (_address == value)
                {
                    return;
                }

                _address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
                AddressChanged?.Invoke(this, new AddressChangedEventArgs(value));
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
                return _hostBrowser.IsLoading;
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
            _hostBrowser.GetBrowser().GoBack();
        }

        public void GoForward()
        {
            _hostBrowser.GetBrowser().GoForward();
        }

        public async Task<string> InvokeScriptAsync(string scriptName, params object[] args)
        {
            var script = new StringBuilder();
            script.Append(scriptName);
            script.Append("(\"");
            for (var i = 0; i < args.Length; i++)
            {
                script.Append(args[i]);
                if (i != args.Length - 1)
                {
                    script.Append(",");
                }
            }
            script.Append("\")");

            var response = await _hostBrowser.EvaluateScriptAsync(script.ToString());
            return response.Result?.ToString();
        }

        public async void Navigate(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            if (_hostBrowser.IsBrowserInitialized == false)
            {
                await _initTcs.Task;
            }

            _hostBrowser.Load(UrlConvert.Convert(url));

            _hostBrowser.GetBrowser().GetHost().SetFocus(true);
        }

        public async void NavigateToString(string html)
        {
            // UNDONE
            // 暂时未找到方法完美实现。
            if (_hostBrowser.IsBrowserInitialized == false)
            {
                await _initTcs.Task;
            }
            _hostBrowser.LoadHtml(html, "about:blank");

            _hostBrowser.GetBrowser().GetHost().SetFocus(true);
        }

        public void Refresh()
        {
            _hostBrowser.Reload();
        }

        public void Stop()
        {
            _hostBrowser.Stop();
        }

        private void HostBrowser_AddressChanged(object sender, CefSharp.AddressChangedEventArgs e)
        {
            Address = e.Address;
        }

        private void HostBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoBack)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoForward)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
        }

        private void HostBrowser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            // 线程回调。
            Dispatcher.Invoke(() =>
            {
                if (Page != null)
                {
                    Page.Title = e.Title;
                }
            });
        }

        private void Init()
        {
            if (ViewModelBase.IsInDesignModeStatic == false)
            {
                // 防止设计器崩溃。
                _hostBrowser = new ChromiumWebBrowser(string.Empty);
                Host.Child = _hostBrowser;
                _hostBrowser.AddressChanged += HostBrowser_AddressChanged;
                _hostBrowser.LoadingStateChanged += HostBrowser_LoadingStateChanged;
                _hostBrowser.TitleChanged += HostBrowser_TitleChanged;

                #region Favicon

                var displayHandler = new WebkitBrowserDisplayHandler();
                displayHandler.FaviconUrlChange += (browserControl, browser, urls) =>
                {
                    // 谁叫 WPF 不原生支持 svg 呢-_-|||
                    var favicon = urls.FirstOrDefault(temp => temp.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                              temp.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                              temp.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                              temp.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
                                                              temp.EndsWith(".gif", StringComparison.OrdinalIgnoreCase));
                    if (favicon != null)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (Page != null)
                            {
                                Page.Favicon = favicon;
                            }
                        });
                    }
                };
                _hostBrowser.DisplayHandler = displayHandler;

                #endregion Favicon

                #region ContextMenu

                var menuHandler = new WebkitContextMenuHandler();
                menuHandler.BeforeContextMenu += (browserControl, browser, frame, parameters, model) =>
                {
                    // 清除默认命令。
                    model.Clear();
                    model.AddItem(CefMenuCommand.Back, "后退");
                    model.SetEnabled(CefMenuCommand.Back, CanGoBack);
                    model.AddItem(CefMenuCommand.Forward, "前进");
                    model.SetEnabled(CefMenuCommand.Forward, CanGoForward);
                    model.AddItem(CefMenuCommand.Reload, "刷新");
                };
                _hostBrowser.MenuHandler = menuHandler;

                #endregion ContextMenu

                #region NewWindow

                var lifeSpanHandler = new WebkitLifeSpanHandler();
                lifeSpanHandler.BeforePopup +=
                    (IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
                        string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture,
                        IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser) =>
                    {
                        newBrowser = null;
                        NewWindowRequest?.Invoke(this, new NewWindowRequestEventArgs(targetUrl));
                        return true;
                    };
                _hostBrowser.LifeSpanHandler = lifeSpanHandler;

                #endregion NewWindow

                #region 调用 Navigate 之前必须确保 Init

                _hostBrowser.IsBrowserInitializedChanged += (sender, e) =>
                {
                    if (e.IsBrowserInitialized)
                    {
                        _initTcs.SetResult(null);
                    }
                };

                #endregion 调用 Navigate 之前必须确保 Init

                WebkitFocusHandler focusHandler = new WebkitFocusHandler();
                focusHandler.GotFocus += () =>
                {
                    AddressBar.RemoveFocus();
                };
                _hostBrowser.FocusHandler = focusHandler;
            }
        }
    }
}