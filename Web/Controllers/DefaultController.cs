using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.IService;
using X.Models;
using X.Web.Base;
using X.Common;

namespace X.Web.Controllers
{
    public class DefaultController : BaseController
    {

        private IUserAccountService userService { get; set; }

        private ILogService logService { get; set; }
        public DefaultController(IUserAccountService _userService, ILogService _logService)
        {
            userService = _userService;
            logService = _logService;
        }

        // GET: Default
        public ActionResult Index()
        {
            var model = logService.GetList();
            var instance = DIContainer.Resolve<ILogService>();
            var model2 = instance.GetList();
            var model3 = model2;
            return View();
        }
    }
}