using System;
using System.Collections;

namespace SoftwareKobo.FireDoge.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 获取 IEnumerable 中是否有元素。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Any(this IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }
    }
}