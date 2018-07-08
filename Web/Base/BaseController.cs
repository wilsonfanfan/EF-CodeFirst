using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using X.Common;
using X.Constant;
using X.IService;
using X.Models;

namespace X.Web.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : Controller
    {
        #region ToJson

        protected JsonResult JsonExt(object data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            if (behavior == JsonRequestBehavior.DenyGet
                && string.Equals(this.Request.HttpMethod, "GET",
                                 StringComparison.OrdinalIgnoreCase))
            {
                //Call JsonResult to throw the same exception as JsonResult
                return new JsonResult();
            }
            return new JsonNetResult()
            {
                Data = data
            };
        }
        #endregion

        #region 返回路径
        public string ReUrl
        {
            get
            {
                var reurl = this.Request["ReUrl"];
                if (!reurl.IsNull())
                {
                    return reurl;
                }
                if (Request.UrlReferrer != null && !Request.UrlReferrer.PathAndQuery.IsNull() && Request.UrlReferrer.PathAndQuery != Request.Url.PathAndQuery)
                {
                    return Request.UrlReferrer.PathAndQuery;
                }
                return Url.Action("Index");
            }
        }
        #endregion

        #region 提示信息！
        /// <summary>
        /// 提示信息！操作回滚！
        /// </summary>
        /// <param name="msg">提示文本</param>
        public ActionResult ShowMsg(string msg)
        {
            return Content("<script>alert(\"" + msg.Replace("\\", "\\\\") + "\");history.back();</script>");
        }
        /// <summary>
        /// 提示信息！转向！
        /// </summary>
        /// <param name="msg">提示文本</param>
        /// <param name="url">转向路径</param>
        public ActionResult ShowMsg(string msg, string url)
        {
            return Content("<script>alert(\"" + msg.Replace("\\", "\\\\") + "\");location.href='" + url + "';</script>");
        }

        /// <summary>
        /// 提示信息！转向！
        /// </summary>
        /// <param name="msg">提示文本</param>
        /// <param name="url">转向路径</param>
        public ActionResult ResponseJs(string msg, string url)
        {
            return JavaScript("<script>alert(\"" + msg.Replace("\\", "\\\\") + "\");location.href='" + url + "';</script>");
        }
        #endregion

        protected override void OnException(ExceptionContext filterContext)
        {
            Write(filterContext.Exception);
        }

        public static void Write(Exception ex)
        {
            string errorMessage = string.Format("方法:{0},异常信息:{1}", ex.TargetSite, ex.Message);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "GET" && filterContext.HttpContext.Request.UrlReferrer != null)
            {
                filterContext.Controller.TempData["backUrl"] = filterContext.HttpContext.Request.UrlReferrer.PathAndQuery;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;
       
        }
     
    }
}