using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public class AddressChangedEventArgs : EventArgs
    {
        public AddressChangedEventArgs(string address)
        {
            Address = address;
        }

        public string Address
        {
            get;
            private set;
        }
    }
}