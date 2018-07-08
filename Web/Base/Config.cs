using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Common;
using X.Common.IO;
using X.Models;

namespace X.Web.Base
{
    /// <summary>
    /// 读取配置文件
    /// </summary>
    public partial class Config
    {
        /// <summary>
        ///  网站配置
        /// </summary>
        public static SiteConfig Site { get { return LoadConfig<SiteConfig>(); } }
        /// <summary>
        ///  邮件发送设置
        /// </summary>
        public static EmailConfig Email { get { return LoadConfig<EmailConfig>(); } }
        /// <summary>
        ///  短信平台设置
        /// </summary>
        public static SmsConfig Sms { get { return LoadConfig<SmsConfig>(); } }
        /// <summary>
        ///  文件上传设置
        /// </summary>
        public static UploadConfig Upload { get { return LoadConfig<UploadConfig>(); } }


        #region 配置
        /// <summary>
        /// 缓存前缀
        /// </summary>
        public const string ConfigPath = "ConfigPath:";
        /// <summary>
        /// 缓存前缀
        /// </summary>
        public const string CacheName = "cacheconfig";
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        public static T LoadConfig<T>() where T : new()
        {
            var _t = typeof(T);
            T model = CacheHelper.Get<T>(CacheName + _t.Name);
            if (model == null)
            {
                String filepath = Util.MapPath(ConfigHelper.AppSettings(ConfigPath + _t.Name));
                model = XmlHelper<T>.Load(filepath);
                CacheHelper.Insert(CacheName, model, filepath);
            }
            return model;
        }
        /// <summary>
        ///  保存配置文件
        /// </summary>
        /// <param name="model">对象</param>
        public static bool SaveConifg<T>(T model) where T : new()
        {
            var _t = typeof(T);
            String filepath = Util.MapPath(ConfigHelper.AppSettings(ConfigPath + _t.Name));
            return XmlHelper<T>.Save(model, filepath);
        }
        #endregion
    }
}