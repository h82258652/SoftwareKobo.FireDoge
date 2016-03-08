using SoftwareKobo.FireDoge.Controls;
using SoftwareKobo.FireDoge.Views;

namespace SoftwareKobo.FireDoge.Models
{
    public class TabPage
    {
        private BrowserView _content;

        private TabHeader _header;

        private WebPage _model;

        public TabPage()
        {
        }

        public TabPage(string address)
        {
            Model.Address = address;
        }

        public BrowserView Content
        {
            get
            {
                if (_content == null)
                {
                    _content = new BrowserView();
                }
                return _content;
            }
        }

        public TabHeader Header
        {
            get
            {
                if (_header == null)
                {
                    _header = new TabHeader();
                }
                return _header;
            }
        }

        public WebPage Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new WebPage();
                }
                return _model;
            }
        }
    }
}