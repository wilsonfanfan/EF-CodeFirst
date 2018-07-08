using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;



namespace X.Common.IO
{
    /// <summary>
    /// 文件辅助操作类
    /// </summary>
    public class FileHelper
    {
        #region 文件操作
        /// <summary>
        /// 文件存在
        /// </summary>
        /// <param name="filename">文件名</param>
        public static bool Exists(string filename)
        {
            return File.Exists(filename);
        }
        /// <summary>
        /// 删除文件（到回收站[可选]）
        /// </summary>
        /// <param name="filename">要删除的文件名</param>
        /// <param name="isSendToRecycleBin">是否删除到回收站</param>
        public static void Delete(string filename, bool isSendToRecycleBin = false)
        {
            if (isSendToRecycleBin)
            {
                FileSystem.DeleteFile(filename, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            else
            {
                File.Delete(filename);
            }
        }

        /// <summary>
        /// 设置或取消文件的指定<see cref="FileAttributes"/>属性
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="attribute">要设置的文件属性</param>
        /// <param name="isSet">true为设置，false为取消</param>
        public static void SetAttribute(string fileName, FileAttributes attribute, bool isSet)
        {
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                throw new FileNotFoundException("要设置属性的文件不存在。", fileName);
            }
            if (isSet)
            {
                fi.Attributes = fi.Attributes | attribute;
            }
            else
            {
                fi.Attributes = fi.Attributes & ~attribute;
            }
        }

        /// <summary>
        /// 获取文件版本号
        /// </summary>
        /// <param name="fileName"> 完整文件名 </param>
        /// <returns> 文件版本号 </returns>
        public static string GetVersion(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
                return fvi.FileVersion;
            }
            return null;
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="fileName"> 文件名 </param>
        /// <returns> 32位MD5 </returns>
        public static string GetFileMd5(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            const int bufferSize = 1024 * 1024;
            byte[] buffer = new byte[bufferSize];

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();

            long offset = 0;
            while (offset < fs.Length)
            {
                long readSize = bufferSize;
                if (offset + readSize > fs.Length)
                {
                    readSize = fs.Length - offset;
                }
                fs.Read(buffer, 0, (int)readSize);
                if (offset + readSize < fs.Length)
                {
                    md5.TransformBlock(buffer, 0, (int)readSize, buffer, 0);
                }
                else
                {
                    md5.TransformFinalBlock(buffer, 0, (int)readSize);
                }
                offset += bufferSize;
            }
            fs.Close();
            byte[] result = md5.Hash;
            md5.Clear();
            StringBuilder sb = new StringBuilder(32);
            foreach (byte b in result)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destFilePath">目标文件路径</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        public static bool MoveFile(String sourceFilePath, String destFilePath, bool overwrite = true)
        {
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException(sourceFilePath + "文件不存在！");
            }
            try
            {
                if (overwrite && File.Exists(destFilePath))
                {
                    File.Delete(destFilePath);
                }
                File.Move(sourceFilePath, destFilePath);
                return true;
            }
            catch
            {
                throw new FileNotFoundException(destFilePath + "文件存在！");
            }
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="destFilePath">目标文件路径</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool CopyFile(string sourceFilePath, string destFilePath, bool overwrite = true)
        {
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException(sourceFilePath + "文件不存在！");

            if (!overwrite && File.Exists(destFilePath))
                return false;

            try
            {
                File.Copy(sourceFilePath, destFilePath, true);
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 文件上传

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="uploadFile">上传文件</param>
        /// <param name="pathDir">上传目录</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileType">验证格式(空为不限制)</param>
        /// <param name="fileSize">文件大小(单位KB,0为不限制)</param>
        /// <param name="response">响应内容</param>
        /// <returns></returns>
        public static FileModel UpLoad(HttpPostedFileBase uploadFile, String pathDir, String fileName, String[] fileType, long fileSize, ref String response)
        {
            if (!pathDir.EndsWith("/"))
            {
                pathDir = pathDir + "/";
            }
            if (!pathDir.StartsWith("/"))
            {
                pathDir = "/" + pathDir;
            }
            pathDir += DateTime.UtcNow.ToString("yyyyMM") + "/";
            var fileExtName = Path.GetExtension(uploadFile.FileName).ToLower();
            if (fileType != null && fileType.Length > 0)
            {
                if (!fileType.Any(a => a.ToLower() == fileExtName))
                {
                    response = "文件格式不正确！支持格式(" + String.Join("|", fileType) + ")";
                    return null;
                }
            }
            if (fileName.IsNull())
            {
                fileName = RandHelper.GetTimeRandom("ddHHmmssffff");
            }
            string filePath = pathDir + fileName + fileExtName;
            return UpLoad(uploadFile, filePath, fileSize, ref  response);
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="uploadFile">上传文件</param>
        /// <param name="filePath">保存物理路径</param>
        /// <param name="fileSize">文件大小(单位KB,0为不限制)</param>
        /// <param name="response">响应内容</param>
        /// <returns></returns>
        public static FileModel UpLoad(HttpPostedFileBase uploadFile, String filePath, long fileSize, ref String response)
        {
            try
            {
                if (uploadFile == null || uploadFile.ContentLength == 0)
                {
                    response = "文件为空！";
                    return null;
                }
                if (filePath.IsNull())
                {
                    response = "保存路径不能为空";
                    return null;
                }
                var model = new FileModel();
                model.Size = uploadFile.ContentLength;
                if (fileSize > 0 && model.Size > fileSize * 1024)
                {
                    response = "文件大小超出限制！";
                    return null;
                }
                model.Name = uploadFile.FileName;
                model.Url = filePath;
                model.Extension = Path.GetExtension(uploadFile.FileName).ToLower();
                model.Path = Util.MapPath(filePath);
                DirectoryHelper.Create(model.Path);
                uploadFile.SaveAs(filePath);
                return model;
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return null;
        }
        #endregion

        #region 文件下载
        /// <summary>
        /// 以指定的ContentType输出指定文件文件
        /// </summary>
        /// <param name="fileName">输出的文件名</param>
        /// <param name="filePath">文件路径</param>
        public static void ResponseFile(string fileName, string filePath)
        {
            ResponseFile(HttpContext.Current.Request, HttpContext.Current.Response, fileName, filePath, 10);
        }
        /// <summary>
        ///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="request">Page.Request对象</param>
        /// <param name="response">Page.Response对象</param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePath">带文件名下载路径</param>
        /// <param name="speed">每秒允许下载的字节数</param>
        /// <returns>返回是否成功</returns>
        public static void ResponseFile(HttpRequest request, HttpResponse response, string fileName, string filePath, long speed)
        {
            Stream iStream = null;
            BinaryReader br = null;
            // 缓冲区为10k
            byte[] buffer = new Byte[10240];
            try
            {
                // 打开文件
                iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                br = new BinaryReader(iStream);
                long fileLength = iStream.Length;
                long startBytes = 0;
                response.AddHeader("Accept-Ranges", "bytes");
                response.Buffer = false;
                int pack = 1024 * 10; //10K bytes
                //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                int sleep = (int)Math.Ceiling((double)(1000 * pack / (speed * 1024)));
                if (request.Headers["Range"] != null)
                {
                    response.StatusCode = 206;
                    string[] range = request.Headers["Range"].Split(new char[] { '=', '-' });
                    startBytes = Convert.ToInt64(range[1]);
                }
                response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                if (startBytes != 0)
                {
                    response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                }
                response.AddHeader("Connection", "Keep-Alive");
                response.ContentType = "application/octet-stream";

                if (request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") > -1)
                {
                    response.AddHeader("Content-Disposition", "attachment;filename=" + fileName.Replace("+", " "));
                }
                else
                {
                    response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                }

                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                for (int i = 0; i < maxCount; i++)
                {
                    if (response.IsClientConnected)
                    {
                        response.BinaryWrite(br.ReadBytes(pack));
                        response.Flush();
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        i = maxCount;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    // 关闭文件
                    iStream.Close();
                }
                br.Close();
                response.End();
            }
        }
        #endregion

        #region 其他
        /// <summary>
        /// 转换文件大小
        /// </summary>
        /// <param name="fileS"></param>
        /// <returns></returns>
        public static String formetFileSize(long fileS)
        {
            String fileSizeString = "";
            if (fileS == 0)
            {
                return "";
            }
            if (fileS < 1024)
            {
                fileSizeString = fileS.ToString() + "B";
            }
            else if (fileS < 1048576)
            {
                fileSizeString = ((double)fileS / 1024).ToString("0.00").TrimEnd('0', '.') + "KB";
            }
            else if (fileS < 1073741824)
            {
                fileSizeString = ((double)fileS / 1048576).ToString("0.00").TrimEnd('0', '.') + "MB";
            }
            else if (fileS < 1099511627776)
            {
                fileSizeString = ((double)fileS / 1073741824).ToString("0.00").TrimEnd('0', '.') + "GB";
            }
            else
            {
                fileSizeString = ((double)fileS / 1073741824).ToString("0.00").TrimEnd('0', '.') + "T";
            }
            return fileSizeString;
        }
        #endregion
    }
}