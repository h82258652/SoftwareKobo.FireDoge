using CefSharp;
using System.Collections.Generic;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public delegate void WebkitAddressChangedHandler(IWebBrowser browserControl, CefSharp.AddressChangedEventArgs addressChangedArgs);

    public delegate bool WebkitConsoleMessageHandler(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs);

    public delegate void WebkitFaviconUrlChangeHandler(IWebBrowser browserControl, IBrowser browser, IList<string> urls);

    public delegate void WebkitFullScreenModeChangeHandler(IWebBrowser browserControl, IBrowser browser, bool fullscreen);

    public delegate void WebkitStatusMessageHandler(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs);

    public delegate void WebkitTitleChangedHandler(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs);

    public delegate bool WebkitTooltipChangedHandler(IWebBrowser browserControl, string text);

    public class WebkitBrowserDisplayHandler : IDisplayHandler
    {
        public event WebkitAddressChangedHandler AddressChanged;

        public event WebkitConsoleMessageHandler ConsoleMessage;

        public event WebkitFaviconUrlChangeHandler FaviconUrlChange;

        public event WebkitFullScreenModeChangeHandler FullScreenModeChange;

        public event WebkitStatusMessageHandler StatusMessage;

        public event WebkitTitleChangedHandler TitleChanged;

        public event WebkitTooltipChangedHandler TooltipChanged;

        public void OnAddressChanged(IWebBrowser browserControl, CefSharp.AddressChangedEventArgs addressChangedArgs)
        {
            AddressChanged?.Invoke(browserControl, addressChangedArgs);
        }

        public bool OnConsoleMessage(IWebBrowser browserControl, ConsoleMessageEventArgs consoleMessageArgs)
        {
            if (ConsoleMessage != null)
            {
                return ConsoleMessage(browserControl, consoleMessageArgs);
            }
            return false;
        }

        public void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, IList<string> urls)
        {
            FaviconUrlChange?.Invoke(browserControl, browser, urls);
        }

        public void OnFullscreenModeChange(IWebBrowser browserControl, IBrowser browser, bool fullscreen)
        {
            FullScreenModeChange?.Invoke(browserControl, browser, fullscreen);
        }

        public void OnStatusMessage(IWebBrowser browserControl, StatusMessageEventArgs statusMessageArgs)
        {
            StatusMessage?.Invoke(browserControl, statusMessageArgs);
        }

        public void OnTitleChanged(IWebBrowser browserControl, TitleChangedEventArgs titleChangedArgs)
        {
            TitleChanged?.Invoke(browserControl, titleChangedArgs);
        }

        public bool OnTooltipChanged(IWebBrowser browserControl, string text)
        {
            if (TooltipChanged != null)
            {
                return TooltipChanged(browserControl, text);
            }
            return false;
        }
    }
}