using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Utils
{
    public static class Favicon
    {
        public static async Task<Uri> GetFaviconAsync(Uri url)
        {
            Elmah.Io.FaviconLoader.Favicon favicon = new Elmah.Io.FaviconLoader.Favicon();
            return await Task.Run(() =>
            {
                try
                {
                    var result = favicon.Load(new Uri(url.Scheme + "://" + url.Host));
                    return result;
                }
                catch (WebException e)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                    return new Uri("http://" + url.Host + "/favicon.ico");
                }
                catch (NotSupportedException e)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                    return null;
                }
            });
        }
    }
}