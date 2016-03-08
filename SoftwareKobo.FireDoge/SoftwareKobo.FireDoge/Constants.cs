using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SoftwareKobo.FireDoge
{
    public static class Constants
    {
        public const string CacheFolderName = "WebCache";

        /// <summary>
        /// 该应用程序的所在路径，含应用程序名称。
        /// </summary>
        public static readonly string AppPath = Assembly.GetEntryAssembly().Location;

        /// <summary>
        /// 应用程序名称，含扩展名。
        /// </summary>
        public static readonly string AppNameWithExtension = Path.GetFileName(AppPath);

        /// <summary>
        /// 缓存文件夹路径。
        /// </summary>
        public static readonly string CacheFolderPath = Path.Combine(Application.StartupPath, CacheFolderName);

        public const string UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36";

        public const string NewTabPageTitle = "新标签页";

        public const string ConfigFileName = "firedog_config.ini";

        public static readonly string ConfigFileFullName = Path.Combine(Application.StartupPath, ConfigFileName);
    }
}