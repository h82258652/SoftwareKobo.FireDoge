using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareKobo.FireDoge.Utils
{
    public static class Hash
    {
        /// <summary>
        /// http://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var buffer = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    buffer.Append(b.ToString("X2"));
                }
                return buffer.ToString();
            }
        }
    }
}