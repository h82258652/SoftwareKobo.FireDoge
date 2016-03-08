using System;
using System.Windows.Forms;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    // http://www.cnblogs.com/yuanjw/archive/2009/02/09/1386789.html
    public class WebBrowserEx : WebBrowser
    {
        private AxHost.ConnectionPointCookie _cookie;

        private WebBrowserExtendedEvents _events;

        //This method will be called to give you a chance to create your own event sink
        protected override void CreateSink()
        {
            //MAKE SURE TO CALL THE BASE or the normal _events won't fire
            base.CreateSink();
            _events = new WebBrowserExtendedEvents(this);
            _cookie = new AxHost.ConnectionPointCookie(ActiveXInstance, _events, typeof(DWebBrowserEvents2));
        }

        protected override void DetachSink()
        {
            if (null != _cookie)
            {
                _cookie.Disconnect();
                _cookie = null;
            }
            base.DetachSink();
        }

        //This new event will fire when the page is navigating
        public event EventHandler BeforeNavigate;

        public event EventHandler BeforeNewWindow;

        protected void OnBeforeNewWindow(string url, out bool cancel)
        {
            var args = new WebBrowserExNavigatingEventArgs(url, null);
            BeforeNewWindow?.Invoke(this, args);
            cancel = args.Cancel;
        }

        protected void OnBeforeNavigate(string url, string frame, out bool cancel)
        {
            WebBrowserExNavigatingEventArgs args = new WebBrowserExNavigatingEventArgs(url, frame);
            BeforeNavigate?.Invoke(this, args);
            //Pass the cancellation chosen back out to the _events
            cancel = args.Cancel;
        }

        //This class will capture _events from the WebBrowser
        private class WebBrowserExtendedEvents : System.Runtime.InteropServices.StandardOleMarshalObject, DWebBrowserEvents2
        {
            private readonly WebBrowserEx _browser;

            public WebBrowserExtendedEvents(WebBrowserEx browser)
            {
                _browser = browser;
            }

            //Implement whichever _events you wish
            public void BeforeNavigate2(object pDisp, ref object url, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
            {
                _browser.OnBeforeNavigate((string)url, (string)targetFrameName, out cancel);
            }

            public void NewWindow3(object pDisp, ref bool cancel, ref object flags, ref object urlContext, ref object url)
            {
                _browser.OnBeforeNewWindow((string)url, out cancel);
            }
        }

        [System.Runtime.InteropServices.ComImport(), System.Runtime.InteropServices.Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"),
        System.Runtime.InteropServices.InterfaceTypeAttribute(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIDispatch),
        System.Runtime.InteropServices.TypeLibType(System.Runtime.InteropServices.TypeLibTypeFlags.FHidden)]
        public interface DWebBrowserEvents2
        {
            [System.Runtime.InteropServices.DispId(250)]
            void BeforeNavigate2(
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
                [System.Runtime.InteropServices.In] ref object url,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object targetFrameName, [System.Runtime.InteropServices.In] ref object postData,
                [System.Runtime.InteropServices.In] ref object headers,
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.Out] ref bool cancel);

            [System.Runtime.InteropServices.DispId(273)]
            void NewWindow3(
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
                [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] ref bool cancel,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object urlContext,
                [System.Runtime.InteropServices.In] ref object url);
        }
    }
}