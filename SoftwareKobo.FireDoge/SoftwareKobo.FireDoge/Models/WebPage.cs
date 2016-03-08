using GalaSoft.MvvmLight;
using SoftwareKobo.FireDoge.Controls;
using SoftwareKobo.FireDoge.Datas;
using SoftwareKobo.FireDoge.Views;

namespace SoftwareKobo.FireDoge.Models
{
    public class WebPage : ObservableObject
    {
        private string _address = AppSetting.IndexPage;

        private string _favicon;

        private string _title = Constants.NewTabPageTitle;

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                Set(ref _address, value);
            }
        }

        public string Favicon
        {
            get
            {
                return _favicon;
            }
            internal set
            {
                Set(ref _favicon, value);
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            internal set
            {
                Set(ref _title, value);
            }
        }
    }
}