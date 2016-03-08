using CefSharp;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public delegate void WebkitGotFocusHandler();

    public delegate bool WebkitSetFocusHandler(CefFocusSource source);

    public delegate void WebkitTakeFocus(bool next);

    public class WebkitFocusHandler : IFocusHandler
    {
        public event WebkitGotFocusHandler GotFocus;

        public event WebkitSetFocusHandler SetFocus;

        public event WebkitTakeFocus TakeFocus;

        public void OnGotFocus()
        {
            GotFocus?.Invoke();
        }

        public bool OnSetFocus(CefFocusSource source)
        {
            if (SetFocus != null)
            {
                return SetFocus(source);
            }
            return false;
        }

        public void OnTakeFocus(bool next)
        {
            TakeFocus?.Invoke(next);
        }
    }
}