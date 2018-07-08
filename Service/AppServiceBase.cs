using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Common;
using X.IDao;
using X.IService;
using X.IService.Authentication;
using X.Models;
using X.Service.Authentication;

namespace X.Service
{
    public class AppServiceBase
    {
        protected UserSession CurrentUserSession
        {
            get
            {
                IAuthenticationService instance = DIContainer.Resolve<IAuthenticationService>();
                var currentUser = instance.GetAuthenticatedUser();
                if (currentUser != null)
                    return currentUser;
                return new UserSession { UserId = 0 };
            }
        }
        /// <summary>  
        /// SQL语句修改
        /// </summary>  
        /// <param name="TableName">表名</param>
        /// <param name="Fields">字段</param>
        /// <param name="Where">条件</param>
        public static String CreateUpdate(String TableName, String Fields, String Where)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("UPDATE {0} SET {1}", TableName, Fields);
            if (Where != null)
            {
                sql.AppendFormat(" WHERE {0}", Where);
            }
            return sql.ToString();
        }

        //#region 日志
        //public void Log(ActionModel actm, string logtype, string explain, long objectId = 0, string pname = "")
        //{
        //    IService.ISysLogService log = DIContainer.Resolve<IService.ISysLogService>();
        //    IPermissionService perm = DIContainer.Resolve<IService.IPermissionService>();

        //    var model = new SysLog { LinkUrl = actm.PathAndQuery, TypeVal = logtype, Explain = explain, ObjectId = objectId };
        //    var currentUser = this.CurrentUserSession;
        //    if (currentUser != null)
        //    {
        //        model.UserId = currentUser.UserId;
        //        model.UserName = currentUser.UserName;
        //    }
        //    model.UserIP = Util.GetIP();
        //    var response = perm.GetPermissionID(actm.ControlName, actm.ActionName, pname, actm.AreaName);
        //    if (response.Status)
        //    {
        //        model.PermissionId = response.Result;
        //    }
        //    log.Create(model);
        //}
        //#endregion
    }
}
