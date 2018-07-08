using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace X.Common
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtension
    {
        #region 判断
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
        /// <summary>
        /// 是否INT
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }
        /// <summary>
        /// 是否INT
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static void CheckNotNullOrEmpty(this string s, string name)
        {
            if (s.IsNull())
            {
                new ArgumentNullException(name);
            }
        }

        
        #endregion

        #region 数据转换
        /// <summary>
        /// 转换INT
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignore">是否忽略异常</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static int ToInt(this string s, bool ignore = true, int def = 0)
        {
            if (ignore)
            {
                int i = 0;
                if (int.TryParse(s, out i))
                {
                    return i;
                }
                return def;
            }
            return int.Parse(s);
        }
        /// <summary>
        /// 转换long
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignore">是否忽略异常</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public static long ToLong(this string s, bool ignore = true, long def = 0)
        {
            if (ignore)
            {
                long i = 0;
                if (long.TryParse(s, out i))
                {
                    return i;
                }
                return def;
            }
            return long.Parse(s);
        }

        /// <summary>
        /// 转换long
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignore">是否忽略异常</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string s, bool ignore = false)
        {
            if (ignore)
            {
                DateTime i = DateTime.UtcNow;
                DateTime.TryParse(s, out i);
                return i;
            }
            return DateTime.Parse(s);
        }
        #endregion

        #region 其他操作
        /// <summary> 
        /// 使用正则表达式删除用户输入中的html内容 
        /// </summary> 
        /// <param name="s">输入内容</param> 
        /// <returns>清理后的文本</returns> 
        public static string ClearHtml(this string s)
        {
            if (s.IsNull())
            {
                return "";
            }
            string pattern = @"(<[a-zA-Z].*?>)|(<[\/][a-zA-Z].*?>)";
            s = Regex.Replace(s, pattern, String.Empty, RegexOptions.IgnoreCase);
            s = s.Replace("<", "&lt;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("&nbsp;", " ");
            return ClearSpace(s);
        }
        /// <summary> 
        /// 使用正则表达式删除连续空格和换行
        /// </summary> 
        /// <param name="s">输入内容</param> 
        /// <returns>清理后的文本</returns> 
        public static string ClearSpace(this string s)
        {
            if (s.IsNull())
            {
                return "";
            }
            s = Regex.Replace(s, "[\f\n\r\t\v]", "");
            s = Regex.Replace(s, " {2,}", " ");
            return s.Trim();
        }
        /// <summary>
        /// 去除前后空白字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimExt(this string s)
        {
            if (s.IsNull())
            {
                return "";
            }
            return s.Trim();
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="FormatStr">时间格式</param>
        /// <returns>处理后字符串 </returns>
        public static string FormatTime(string s, string FormatStr = "yyyy-MM-dd")
        {
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(s, out dt))
            {
                return dt.ToString(FormatStr);
            }
            return s;
        }
        /// <summary> 
        /// 字符串截取函数，截取左边指定的字节数 
        /// </summary> 
        /// <param name="s">字段串</param>
        /// <param name="CutLength">截取长度</param> 
        /// <param name="clearhtml">是否清空Html</param> 
        /// <param name="bl">是否添加省略号</param> 
        /// <returns>返回处理后的字符串</returns> 
        public static string CutStr(this string s, int CutLength, bool clearhtml, bool bl)
        {
            if (IsNull(s) || CutLength <= 0)
            {
                return "";
            }
            int iCount = GetStringLength(s);
            if (iCount > CutLength)
            {
                int iLength = 0;
                if (bl)
                {
                    CutLength = CutLength - 3;
                }
                for (int i = 0; i < s.Length; i++)
                {
                    int iCharLength = Encoding.Default.GetByteCount(new char[] { s[i] });
                    iLength += iCharLength;
                    if (iLength == CutLength)
                    {
                        s = s.Substring(0, i + 1);
                        break;
                    }
                    else if (iLength > CutLength)
                    {
                        s = s.Substring(0, i);
                        break;
                    }
                }
                if (bl)
                {
                    s += "...";
                }
            }
            return s;
        }

        public static string Base64Encode(this string plainText, Encoding encoding)
        {
            var plainTextBytes = encoding.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string encodedString, Encoding encoding)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            return encoding.GetString(data);
        }
        /// <summary>
        /// 为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// 将字符串反转
        /// </summary>
        /// <param name="s">要反转的字符串</param>
        public static string ReverseString(this string s)
        {
            return new string(s.Reverse().ToArray());
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="s">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string s, string strSplit, bool removeEmptyEntries = false)
        {
            return s.Split(new[] { strSplit }, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// 支持汉字的字符串长度，汉字长度计为2
        /// </summary>
        /// <param name="s">参数字符串</param>
        /// <returns>当前字符串的长度，汉字长度为2</returns>
        public static int GetStringLength(this string s)
        {
            return Encoding.Default.GetBytes(s).Length;
        }

        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string s, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(s);
        }

        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string ToString(this byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        #endregion

        #region 正则表达式

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="s">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s.IsNull())
            {
                return false;
            }
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="s">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string s, string pattern)
        {
            if (s.IsNull())
            {
                return null;
            }
            return Regex.Match(s, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="s"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合，包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> Matches(this string s, string pattern)
        {
            if (s.IsNull())
            {
                return new string[] { };
            }
            MatchCollection matches = Regex.Matches(s, pattern);
            return from Match match in matches select match.Value;
        }

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string s)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return s.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string s)
        {
            const string pattern = @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
            return s.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string s)
        {
            const string pattern = @"^\-?[0-9]+$";
            return s.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string s)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return s.IsMatch(pattern);
        }

        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string s)
        {
            const string pattern = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return s.IsMatch(pattern);
        }

        /// <summary>
        /// 是否身份证号，验证如下3种情况：
        /// 1.身份证号码为15位数字；
        /// 2.身份证号码为18位数字；
        /// 3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCard(this string s)
        {
            const string pattern = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return s.IsMatch(pattern);
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string s, bool isRestrict = false)
        {
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return s.IsMatch(pattern);
        }

        #endregion

        #region list string

        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speaters">分隔符</param>
        /// <returns></returns>
        public static List<int> GetIntList(this string str, params string[] speaters)
        {
            try
            {
                if (speaters == null)
                {
                    return null;
                }
                return str.Split(speaters, StringSplitOptions.RemoveEmptyEntries).Select<string, int>(q => int.Parse(q.Trim())).ToList();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speaters">分隔符</param>
        /// <returns></returns>
        public static List<long> GetLongList(this string str, params string[] speaters)
        {
            try
            {
                if (speaters == null)
                {
                    return null;
                }
                return str.Split(speaters, StringSplitOptions.RemoveEmptyEntries).Select<string, long>(q => long.Parse(q.Trim())).ToList();
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
