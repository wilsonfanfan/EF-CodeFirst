using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Common
{
    /// <summary>
    /// 时间扩展
    /// </summary>
    public static class TimeExtension
    {
        public static int GetAge(this DateTime time, DateTime now)
        {
            int age = now.Year - time.Year;
            if (now.Month < time.Month || (now.Month == time.Month && now.Day < time.Day)) age--;
            return age;
        }
        #region 数据处理
        /// <summary>
        /// 转换本地时间字符串
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToLocalDate(this DateTime time, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return time.ToLocalTime().ToString(format);
            //return time.ToString(format);
        }
        /// <summary>
        /// 转换UTC时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime LocalToUTC(this string time)
        {
            return DateTime.Parse(time).ToUniversalTime();
        }
        /// <summary>
        /// 转换本地时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime UTC2Local(this DateTime time)
        {
            return time.ToLocalTime();
        }
        #endregion
    }
}
