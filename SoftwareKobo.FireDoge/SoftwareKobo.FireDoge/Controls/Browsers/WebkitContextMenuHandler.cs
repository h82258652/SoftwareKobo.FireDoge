using CefSharp;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public delegate void WebkitBeforeContextMenuHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model);

    public delegate bool WebkitContextMenuCommandHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags);

    public delegate void WebkitContextMenuDismissedHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame);

    public delegate bool WebkitRunContextMenuHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback);

    public class WebkitContextMenuHandler : IContextMenuHandler
    {
        public WebkitContextMenuHandler()
        {
            /*
             * 先执行 BeforeContextMenu
             * 再执行 RunContextMenu
             * 最后执行 ContextMenuDismissed
             * ContextMenuCommand 为命令执行。
             */
        }

        public event WebkitBeforeContextMenuHandler BeforeContextMenu;

        public event WebkitContextMenuCommandHandler ContextMenuCommand;

        public event WebkitContextMenuDismissedHandler ContextMenuDismissed;

        public event WebkitRunContextMenuHandler RunContextMenuRequested;

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            BeforeContextMenu?.Invoke(browserControl, browser, frame, parameters, model);
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (ContextMenuCommand != null)
            {
                return ContextMenuCommand(browserControl, browser, frame, parameters, commandId, eventFlags);
            }
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            ContextMenuDismissed?.Invoke(browserControl, browser, frame);
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            if (RunContextMenuRequested != null)
            {
                return RunContextMenuRequested(browserControl, browser, frame, parameters, model, callback);
            }
            return false;
        }
    }
}