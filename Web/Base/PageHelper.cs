using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using X.Common;
using X.Models;
using System.Web.Mvc.Html;

namespace X.Web.Base
{
    public static class PageHelper
    {
        #region 请求转换
        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static ActionModel GetActionObj(this RouteData obj)
        {
            var model = new ActionModel();
            model.ActionName = obj.Values["action"].ToString();
            model.ControlName = obj.Values["controller"].ToString();
            model.AreaName = GetAreaName(obj);
            return model;
        }

        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        private static string GetAreaName(RouteData routeData)
        {
            object area;
            if (routeData.DataTokens.TryGetValue("area", out area))
            {
                return area as string;
            }
            return GetAreaName(routeData.Route);
        }

        /// <summary>
        /// 从路由数据获取AreaName
        /// </summary>
        /// <param name="route"><see cref="RouteBase"/></param>
        /// <returns>返回路由中的AreaName，如果无AreaName则返回null</returns>
        private static string GetAreaName(RouteBase route)
        {
            IRouteWithArea routeWithArea = route as IRouteWithArea;
            if (routeWithArea != null)
            {
                return routeWithArea.Area;
            }

            Route castRoute = route as Route;
            if (castRoute != null && castRoute.DataTokens != null)
            {
                return castRoute.DataTokens["area"] as string;
            }
            return null;
        }
        #endregion


        #region 路径
        /// <summary>
        /// 得到插件路径
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String Plugin(this UrlHelper helper, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "";
            }

            if (path.StartsWith("~"))
                return helper.Content(path);
            else
                return helper.Content("~/Plugins/" + path);
        }

        /// <summary>
        /// 得到主题
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String Theme(this UrlHelper helper, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "";
            }

            if (path.StartsWith("~"))
                return helper.Content(path);
            else
                return helper.Content(ConfigHelper.AppSettings("Theme") + path);
        }
        /// <summary>
        /// 得到JS
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String Js(this UrlHelper helper, string name)
        {
            return Theme(helper, "Js/" + name);
        }
        /// <summary>
        /// 得到Css
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String Style(this UrlHelper helper, string name)
        {
            return Theme(helper, "Styles/" + name);
        }
        /// <summary>
        /// 得到图片
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String Img(this UrlHelper helper, string name)
        {
            return Theme(helper, "Images/" + name);
        }
        #endregion

        #region 显示状态
        public static MvcHtmlString DisplayStatus(this HtmlHelper html, bool whether)
        {
            if (whether)
            {
                return new MvcHtmlString("<i class='glyphicon glyphicon-ok'></i>");
            }
            return new MvcHtmlString("<i class='glyphicon glyphicon-remove'></i>");
        }
        #endregion

        #region 权限验证
        public static MvcHtmlString CheckLink(this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName = "",
           object routeValues = null,
            object htmlAttributes = null)
        {
            MvcHtmlString link = MvcHtmlString.Empty;
            var actm = helper.ViewContext.RequestContext.RouteData.GetActionObj();
            var areaName = actm.AreaName;
            if (controllerName.IsNull())
            {
                controllerName = actm.ControlName;
            }
            return link;
        }
        public static MvcHtmlString CheckLink(this WebViewPage helper,
            string linkText,
            string actionName,
            string controllerName = "",
            string name = "")
        {
            MvcHtmlString link = MvcHtmlString.Empty;
            var actm = helper.ViewContext.RequestContext.RouteData.GetActionObj();
            var areaName = actm.AreaName;
            if (controllerName.IsNull())
            {
                controllerName = actm.ControlName;
            }
         
            return link;
        }
        public static string CheckLink(this UrlHelper helper,
            string actionName,
            string controllerName = "",
            string name = "")
        {
            string link = "#";
            var actm = helper.RequestContext.RouteData.GetActionObj();
            var areaName = actm.AreaName;
            if (controllerName.IsNull())
            {
                controllerName = actm.ControlName;
            }
          
            return link;
        }
        #endregion
    }
}