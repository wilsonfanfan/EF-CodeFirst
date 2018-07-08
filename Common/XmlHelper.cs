using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace X.Common
{
    /// <summary>
    /// Xml序列化
    /// </summary>
    public class XmlHelper<T> where T : new()
    {
        private static object lockHelper = new object();

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="str">xml内容</param>
        /// <returns></returns>
        public static T StringConvert(string str)
        {
            try
            {
                using (StringReader strRdr = new StringReader(str))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(strRdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static T Load(string filepath)
        {
            FileStream fs = null;
            try
            {
                if (!File.Exists(filepath))
                {
                    return new T();
                }
                fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static bool Save(T obj, string filename)
        {
            lock (lockHelper)
            {
                FileStream fs = null;
                // serialize it...
                try
                {
                    fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(fs, obj);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
            //return false;
        }
    }
}
