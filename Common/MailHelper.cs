using System;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace X.Common
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        public MailHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private static string _SendEmail;
        private static string _SendEmailPwd;
        private static string _SendSmtp;

        #region  发送邮件 参数
        /// <summary>
        /// 发送邮箱地址
        /// <add key="SendEmail" value="admin@kingspeed.com"/>
        /// </summary>
        public static string SendEmail
        {
            get
            {
                if (String.IsNullOrEmpty(_SendEmail))
                {
                    _SendEmail = ConfigurationManager.AppSettings["SendEmail"];
                }
                if (String.IsNullOrEmpty(_SendEmail))
                {
                    _SendEmail = "admin@kingspeed.com";
                }
                return _SendEmail;
            }
            set { MailHelper._SendEmail = value; }
        }

        /// <summary>
        /// 发送邮箱密码
        /// <add key="SendEmailPwd" value="yuchong1234#"/>
        /// </summary>
        public static string SendEmailPwd
        {
            get
            {
                if (String.IsNullOrEmpty(_SendEmailPwd))
                {
                    _SendEmailPwd = ConfigurationManager.AppSettings["SendEmailPwd"];
                }
                if (String.IsNullOrEmpty(_SendEmailPwd))
                {
                    _SendEmailPwd = "yunxia0621";
                }
                return _SendEmailPwd;
            }
            set { MailHelper._SendEmailPwd = value; }
        }
        /// <summary>
        /// 发送SMTP服务器
        /// <add key="SendSmtp" value="smtp.163.com"/>
        /// </summary>
        public static string SendSmtp
        {
            get
            {
                if (String.IsNullOrEmpty(_SendSmtp))
                {
                    _SendSmtp = ConfigurationManager.AppSettings["SendSmtp"];
                }
                if (String.IsNullOrEmpty(_SendSmtp))
                {
                    _SendSmtp = "smtp.exmail.qq.com";
                }
                return _SendSmtp;
            }
            set { MailHelper._SendSmtp = value; }
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="ToEmail">接收邮箱,多邮箱','隔开</param>
        /// <param name="msgTitle">主题</param>
        /// <param name="msgBody">内容</param>
        /// <param name="Alias">别名</param>
        /// <param name="File">附件,多附件','隔开</param>
        /// <param name="IsEnableSSL">是否开启SSL</param>
        /// <returns>返回bool </returns>
        public static bool Send(string ToEmail, string msgTitle, string msgBody, string Alias = "", string File = "", bool IsEnableSSL = false)
        {
            bool flag = false;
            if(String.IsNullOrEmpty(ToEmail))
            {
                return flag;
            }

            MailMessage MM = new MailMessage();
            if(Alias.IsNull())
            {
                MM.From = new MailAddress(SendEmail);  
            }
            else
            {
                MM.From = new MailAddress(SendEmail, Alias);    //发件人，发件人别名
            }
            //收件人集合
            string[] mail = ToEmail.Split(',');
            for (int i = 0; i < mail.Length; i++)
            {
                MM.To.Add(mail[i].ToString());
            }
            MM.SubjectEncoding = Encoding.UTF8;     //主题编码
            MM.Subject = msgTitle;                  //主题
            MM.Body = msgBody;                      //内容
            MM.BodyEncoding = Encoding.UTF8;        //内容编码
            MM.IsBodyHtml = true;                   //设置邮件为HTML格式
            MM.Priority = MailPriority.Normal;      //优先级
            //附件集合
            if (!String.IsNullOrEmpty(File))
            {
                string[] files = File.Split(',');
                for (int j = 0; j < files.Length; j++)
                {
                    MM.Attachments.Add(new Attachment(files[j].ToString()));
                }
            }

            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;  //指定电子邮件发送方式
            smtp.Host = SendSmtp;                               //指定SMTP服务器
            smtp.Credentials = new NetworkCredential(SendEmail, SendEmailPwd);       //用户名和密码
            smtp.EnableSsl = IsEnableSSL; //开启SSL

            try
            {
                smtp.Send(MM);
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }
        #endregion
    }
}
