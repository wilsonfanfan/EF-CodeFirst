using System;
using System.Drawing;

namespace X.Common
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public class VerifyCodeHelper : IDisposable
    {

        #region 私有字段
        private string codeStr;
        private Bitmap image;
        private Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
        private string[] font = { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact", "Georgia", "Comic Sans MS" };
        #endregion

        #region 公有属性
        /// <summary>
        /// 验证码字符串
        /// </summary>
        public string CodeStr { get; set; }

        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image { get { return this.image; } }
        /// <summary>
        /// 验证码限制
        /// </summary>
        public int letterLimit { get; set; }
        /// <summary>
        /// 单个字体的宽度范围
        /// </summary>
        public int letterWidth { get; set; }
        /// <summary>
        /// 单个字体的高度范围
        /// </summary>
        public int letterHeight { get; set; }
        /// <summary>
        /// 单个字体的高度范围
        /// </summary>
        public VerifyCodeType codeType { get; set; }
        /// <summary>
        /// 噪线数量
        /// </summary>
        public int noiseNumber { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isInit">默认初始化</param>
        public VerifyCodeHelper(bool isInit = true)
        {
            letterLimit = 5;
            letterWidth = 32;
            letterHeight = 32;
            codeType = VerifyCodeType.Normal;
            noiseNumber = 2;
            if (isInit)
            {
                GenerateVerifyCode();
                CreateImage();
            }
        }
        #endregion

        #region 生成验证码字符串
        /// <summary>
        /// 生成验证码字符串
        /// </summary>
        public void GenerateVerifyCode()
        {
            switch (codeType)
            {
                case VerifyCodeType.Normal:
                default:
                    codeStr = RandHelper.GetRandomStr(letterLimit, true);
                    CodeStr = codeStr;
                    break;
                case VerifyCodeType.PlusAndMinus:
                    int intFirst = RandHelper.GetRandomInt(1, letterLimit);
                    int intSec = RandHelper.GetRandomInt(1, letterLimit);
                    int intIs = RandHelper.GetRandomInt(2);
                    switch (intIs)
                    {
                        case 1:
                            if (intFirst < intSec)
                            {
                                int intTemp = intFirst;
                                intFirst = intSec;
                                intSec = intTemp;
                            }
                            codeStr = intFirst + "-" + intSec + "=";
                            CodeStr = (intFirst - intSec).ToString();
                            break;
                        default:
                            codeStr = intFirst + "+" + intSec + "=";
                            CodeStr = (intFirst + intSec).ToString();
                            break;
                    }
                    codeStr = RandHelper.GetRandomStr(letterLimit, true);
                    break;
            }
        }
        #endregion

        #region 生成验证码图片
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        public void CreateImage()
        {
            int int_ImageWidth = (int)Math.Ceiling(this.codeStr.Length * letterWidth * 1.1);
            int int_ImageHeight = (int)Math.Ceiling(letterHeight * 1.1) + 8;
            image = new Bitmap(int_ImageWidth, int_ImageHeight);
            Graphics g = Graphics.FromImage(image);
            try
            {
                g.Clear(Color.White);
                //画噪线 
                for (int i = 0; i < noiseNumber; i++)
                {
                    int x1 = RandHelper.GetRandomInt(image.Width - 1);
                    int x2 = RandHelper.GetRandomInt(image.Width - 1);
                    int y1 = RandHelper.GetRandomInt(image.Height - 1);
                    int y2 = RandHelper.GetRandomInt(image.Height - 1);
                    Color clr = color[RandHelper.GetRandomInt(color.Length)];
                    g.DrawLine(new Pen(clr), x1, y1, x2, y2);
                }
                float int_x = 0;
                //画验证码字符串 
                for (int i = 0; i < this.codeStr.Length; i++)
                {
                    int_x += RandHelper.GetRandomInt(3);
                    string fnt = font[RandHelper.GetRandomInt(font.Length)];
                    Font ft = new Font(fnt, RandHelper.GetRandomInt(letterHeight - 1, letterHeight + 2));
                    Color clr = color[RandHelper.GetRandomInt(color.Length)];
                    g.DrawString(this.codeStr[i].ToString(), ft, new SolidBrush(clr), int_x, 0);
                    int_x += letterWidth;
                }
                //画噪点 
                for (int i = 0; i < 100; i++)
                {
                    int x = RandHelper.GetRandomInt(image.Width);
                    int y = RandHelper.GetRandomInt(image.Height);
                    Color clr = color[RandHelper.GetRandomInt(color.Length)];
                    image.SetPixel(x, y, clr);
                }
            }
            finally
            {
                //显式释放资源 
                g.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            image.Dispose();
        }
    }
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerifyCodeType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 正常
        /// </summary>
        PlusAndMinus
    }
}
