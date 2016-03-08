using CefSharp;
using System.Text;
using System.Windows;

namespace SoftwareKobo.FireDoge
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("爆了 -_-|||");
            sb.AppendLine(e.Exception.Message);
            sb.AppendLine(e.Exception.StackTrace);
            MessageBox.Show(sb.ToString());
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            // 必须调用这句，否则应用程序无法彻底退出。
            Cef.Shutdown();
        }
    }
}