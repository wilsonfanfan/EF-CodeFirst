using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Common
{
    /// <summary>
    /// 随机类
    /// </summary>
    public class RandHelper
    {
        /// <summary>
        /// 生成器
        /// </summary>
        private static Random random = new Random(DateTime.Now.Second);

        #region 生成一个指定范围的随机整数
        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            return random.Next(minNum, maxNum);
        }
        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数
        /// </summary>
        /// <param name="maxNum">最大值</param>
        public static int GetRandomInt(int maxNum)
        {
            return random.Next(maxNum);
        }
        #endregion

        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            return random.NextDouble();
        }
        #endregion

        #region 生成随机数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string GetRandomNumber(int length)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                result.Append(random.Next(10).ToString());
            }
            return result.ToString();
        }
        #endregion

        #region 生成随机字母与数字

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="noMix">是否去除容易混淆的字符</param>
        public static string GetRandomStr(int length, bool noMix = false)
        {
            char[] pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            if (noMix)
            {
                pattern = new char[] { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            }
            int n = pattern.Length;
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result.Append(pattern[rnd]);
            }
            return result.ToString();
        }
        #endregion

        #region 生成随机纯字母随机数
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string GetRandomChar(int length)
        {
            char[] pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = pattern.Length;
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
        #endregion


        /// <summary> 
        /// 时间年月日时分秒+随机数(1-999)
        /// </summary> 
        public static string GetTimeRandom(String format = "yyyyMMddHHmmss", int length = 3)
        {
            StringBuilder strbder = new StringBuilder();
            strbder.Append(DateTime.UtcNow.ToString(format));
            strbder.Append(GetRandomNumber(length));
            return strbder.ToString();
        }
    }
}
