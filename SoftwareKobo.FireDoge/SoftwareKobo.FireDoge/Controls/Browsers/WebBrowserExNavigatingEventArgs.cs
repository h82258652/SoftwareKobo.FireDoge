using System.ComponentModel;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public class WebBrowserExNavigatingEventArgs : CancelEventArgs
    {
        private string _frame;

        private string _url;

        public WebBrowserExNavigatingEventArgs(string url, string frame)
        {
            _url = url;
            _frame = frame;
        }

        public string Frame
        {
            get
            {
                return _frame;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
        }
    }
}