using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Controls.Browsers
{
    public delegate void AddressChangedEventHandler(IBrowserControl browser, AddressChangedEventArgs e);

    public delegate void NewWindowRequestEventHandler(IBrowserControl browser, NewWindowRequestEventArgs e);

    public interface IBrowserControl
    {
        event AddressChangedEventHandler AddressChanged;

        event NewWindowRequestEventHandler NewWindowRequest;

        string Address
        {
            get;
        }

        bool CanGoBack
        {
            get;
        }

        bool CanGoForward
        {
            get;
        }

        bool IsLoading
        {
            get;
        }

        void GoBack();

        void GoForward();

        Task<string> InvokeScriptAsync(string scriptName, params object[] args);

        void Navigate(string url);

        void NavigateToString(string html);

        void Refresh();

        void Stop();
    }
}