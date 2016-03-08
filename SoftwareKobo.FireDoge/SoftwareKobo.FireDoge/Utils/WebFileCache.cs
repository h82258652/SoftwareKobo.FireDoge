using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Utils
{
    public static class WebFileCache
    {
        public static async Task<string> GetFilePathAsync(string url)
        {
            var uniqueFileName = GenerateUniqueFileName(url);
            var cacheFilePath = Path.Combine(Constants.CacheFolderPath, uniqueFileName);

            if (File.Exists(cacheFilePath))
            {
                // 缓存文件存在，返回文件路径。
                return cacheFilePath;
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Constants.UserAgent);

                try
                {
                    // 下载文件。
                    var bytes = await client.GetByteArrayAsync(url);

                    if (bytes.Length > 0)
                    {
                        // 确保文件夹存在。
                        Directory.CreateDirectory(Constants.CacheFolderPath);
                        File.WriteAllBytes(cacheFilePath, bytes);
                    }

                    return cacheFilePath;
                }
                catch (Exception e)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 由 url 参数唯一的文件名。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GenerateUniqueFileName(string url)
        {
            var extension = Path.GetExtension(url);
            return Hash.GetMd5(url) + extension;
        }
    }
}