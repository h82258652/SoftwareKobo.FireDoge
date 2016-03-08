using CefSharp;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public delegate bool BeforePopupHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser);

    public delegate void AfterCreatedHandler(IWebBrowser browserControl, IBrowser browser);

    public delegate bool DoCloseHandler(IWebBrowser browserControl, IBrowser browser);

    public delegate void BeforeCloseHandler(IWebBrowser browserControl, IBrowser browser);

    public class WebkitLifeSpanHandler : ILifeSpanHandler
    {
        public event BeforePopupHandler BeforePopup;

        public event AfterCreatedHandler AfterCreated;

        public event DoCloseHandler DoCloseRequest;

        public event BeforeCloseHandler BeforeClose;

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName,
            WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess,
            out IWebBrowser newBrowser)
        {
            newBrowser = null;
            if (BeforePopup != null)
            {
                return BeforePopup(browserControl, browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, windowInfo, ref noJavascriptAccess, out newBrowser);
            }
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            AfterCreated?.Invoke(browserControl, browser);
        }

        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            if (DoCloseRequest != null)
            {
                return DoCloseRequest(browserControl, browser);
            }
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            BeforeClose?.Invoke(browserControl, browser);
        }
    }
}