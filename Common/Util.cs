using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace X.Common
{
    public class Util
    {
        #region 获取路径
        /// <summary>
        /// 获得当前物理路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>物理路径</returns>
        public static string MapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                if (strPath.StartsWith("/"))
                {
                    strPath = "~" + strPath;
                }
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
        }
        /// <summary>
        /// 获取站点根目录URL
        /// </summary>
        /// <param name="forumPath">虚拟路径</param>
        /// <returns></returns>
        public static string GetRootUrl(string forumPath)
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}",
                                 HttpContext.Current.Request.Url.Scheme,
                                 HttpContext.Current.Request.Url.Host.ToString(),
                                 (port == 80 || port == 0) ? "" : ":" + port,
                                 forumPath);
        }
        /// <summary>
        /// 获取Email主机
        /// </summary>
        /// <param name="strEmail">E-mail</param>
        /// <returns></returns>
        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (result.IsNull())
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (result.IsNull())
                result = HttpContext.Current.Request.UserHostAddress;

            if (result.IsNull() || !result.IsIpAddress())
                return "";

            return result;
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        public static DateTime UtcDateTime
        {
            get
            {
                return DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            }
        }
        #endregion
    }
}
