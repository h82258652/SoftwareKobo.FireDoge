using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Utils
{
    // 确保返回为url
    public class UrlConvert
    {
        public static string Convert(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                // 直接就是
                return url;
            }

            try
            {
                IPHostEntry entry = Dns.GetHostEntry(url);
                return "http://" + url;
            }
            catch (Exception)
            {
            }

            // 转换为百度搜索好了 →_→
            return "http://www.baidu.com/s?wd=" + url;
        }
    }
}