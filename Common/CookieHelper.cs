using System;
using System.Web;

namespace X.Common
{
    /// <summary>
    /// Cookie操作类
    /// </summary>
    public class CookieHelper
    {

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = HttpUtility.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="key">键</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = HttpUtility.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = HttpUtility.UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                string val = HttpContext.Current.Request.Cookies[strName].Value.ToString();
                if (val == "")
                {
                    return "";
                }
                return HttpUtility.UrlDecode(val);
            }
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="key">键</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                string val = HttpContext.Current.Request.Cookies[strName][key].ToString();
                if (val == "")
                {
                    return "";
                }
                return HttpUtility.UrlDecode(val);
            }
            return "";
        }
        /// <summary>
        /// 删除cookie
        /// </summary>
        public static void Delete(string strName)
        {
            HttpContext.Current.Response.Cookies[strName].Expires = DateTime.Now.AddYears(-10);
        }

        /// <summary>
        /// 清空cookie
        /// </summary>
        public static void Clear()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = HttpContext.Current.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }
    }
}
