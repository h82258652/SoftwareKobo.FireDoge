using SoftwareKobo.FireDoge.Models;
using SoftwareKobo.FireDoge.Utils;
using System;

namespace SoftwareKobo.FireDoge.Datas
{
    public static class AppSetting
    {
        public static BrowserEngine DefaultBrowserEngine
        {
            get
            {
                var storage = Ini.Read(nameof(DefaultBrowserEngine), null);
                BrowserEngine value;
                if (Enum.TryParse(storage, true, out value))
                {
                    return value;
                }
                else
                {
                    return BrowserEngine.Ms;
                }
            }
            set
            {
                Ini.Write(nameof(DefaultBrowserEngine), value.ToString());
            }
        }

        public static string IndexPage
        {
            get
            {
                return Ini.Read(nameof(IndexPage), "http://www.baidu.com/");
            }
            set
            {
                Ini.Write(nameof(IndexPage), value);
            }
        }
    }
}