using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Xml;
namespace X.Common
{
    public class ConfigHelper
    {
        public static string AppSettings(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (value.IsNull())
            {
                return "";
            }
            return value.Trim();
        }
        public static string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString.Trim();
        }

        /// <summary>
        ///  读取配置文件
        /// </summary>
        public static T LoadConfig<T>() where T : new()
        {
            var tname = typeof(T).Name;
            var cachename = "ConfigFile_" + tname;
            T model = CacheHelper.Get<T>(cachename);
            if (model == null)
            {
                String filepath = Util.MapPath("~/App_Data/" + tname + ".config");
                model = XmlHelper<T>.Load(filepath);
                CacheHelper.Insert(cachename, model, filepath);
            }
            return model;
        }

        /// <summary>
        ///  保存配置文件
        /// </summary>
        public static bool SaveConifg<T>(T model) where T : new()
        {
            var tname = typeof(T).Name;
            String filepath = Util.MapPath("~/App_Data/" + tname + ".config");
            return XmlHelper<T>.Save(model, filepath);
        }
    }
}
