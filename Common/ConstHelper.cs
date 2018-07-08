
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace X.Common
{
    public class ConstHelper
    {
        /// <summary>
        /// 获取常量列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetConstList(Type type)
        {
            Dictionary<string, string> dc = new Dictionary<string, string>();
            foreach (FieldInfo item in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly))
            {
                DescriptionAttribute[] arrDesc = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (arrDesc.Length > 0)
                    dc.Add(item.Name, arrDesc[0].Description);
            }
            return dc;
        }

        /// <summary>
        /// 获取中文描述
        /// </summary>
        /// <param name="field">常量名称</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetConstDescription(string field, Type type)
        {
            if (string.IsNullOrEmpty(field))
            {
                return null;
            }
            FieldInfo item = type.GetField(field);
            if (item == null)
            {
                return null;
            }
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (arrDesc.Length > 0)
                return arrDesc[0].Description ?? "";
            return string.Empty;
        }
    }
}
