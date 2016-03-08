using System.Runtime.InteropServices;
using System.Text;

namespace SoftwareKobo.FireDoge.Utils
{
    public class Ini
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName,
            string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        private const string AppName = "FireDoge";

        public static string Read(string propertyName, string defaultValue)
        {
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(AppName, propertyName, defaultValue, sb, (uint)sb.Capacity,
                Constants.ConfigFileFullName);
            return sb.ToString();
        }

        public static bool Write(string propertyName, string value)
        {
            return WritePrivateProfileString(AppName, propertyName, value, Constants.ConfigFileFullName);
        }
    }
}