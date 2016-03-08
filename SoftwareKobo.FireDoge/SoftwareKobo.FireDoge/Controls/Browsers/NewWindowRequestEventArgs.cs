using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public class NewWindowRequestEventArgs : EventArgs
    {
        public NewWindowRequestEventArgs(string url)
        {
            RequestUrl = url;
        }

        public string RequestUrl
        {
            get;
            private set;
        }
    }
}