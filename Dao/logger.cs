using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Dao
{
    /// <summary>
    /// 日志
    /// </summary>
    partial class logger
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="sqlname">数据库连接名称</param>
        /// <param name="sql">sql语句</param>
        /// <param name="ex">异常消息</param>
        public static void Write(string sqlname, string sql, DbException ex = null)
        {
            StringBuilder log = new StringBuilder();
            string path = string.Format("App_Data/Runtime/Sql/{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));

            path = path.Replace("/", "\\");
            if (path.StartsWith("\\"))
            {
                path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
            }
            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            using (StreamWriter w = File.AppendText(path))
            {
                log.Append(sql);
                //log.AppendFormat("{0} --  {1}\r\n", sqlname, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //log.AppendFormat("执行SQL语句:\r\n{0}\r\n", sql);
                if (ex != null)
                {
                    log.AppendFormat("错误如下:\r\n");
                    log.AppendFormat("出错信息：{0}\r\n出错来源：{1}\r\n程序：{2}\r\n异常方法：{3}\r\n", ex.Message, ex.Source, ex.ErrorCode, ex.TargetSite);
                }
                //log.Append("\r\n\r\n------------------------------------------\r\n\r\n");
                w.WriteLine(log.ToString());
                w.Flush();
            }
        }
    }
}
