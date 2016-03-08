namespace SoftwareKobo.FireDoge.Controls
{
    public class AddressBarSubmitedEventArgs
    {
        public AddressBarSubmitedEventArgs(string address)
        {
            SubmitedAddress = address;
        }

        public string SubmitedAddress
        {
            get;
            private set;
        }
    }
}