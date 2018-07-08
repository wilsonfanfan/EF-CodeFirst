using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Models
{
    /// <summary>
    /// 文件上传设置
    /// </summary>
    public class UploadConfig
    {
        public UploadConfig()
        {
            FilePath = "/upload";
            AttachExtension = "gif,jpg,png,bmp,rar,zip,doc,xls,txt";
            AttachSize = 51200;
            VideoExtension = "flv,mp3,mp4,avi";
            VideoSize = 102400;
            ImgExtension = "gif,jpg,png";
            ImgSize = 10240;
            ImgMaxHeight = 1000;
            ImgMaxWidth = 1000;
            ThumbnailHeight = 300;
            ThumbnailWidth = 300;
            WatermarkType = 0;
            WatermarkPosition = 0;
            WatermarkimgQuality = 80;
            WatermarkPic = "watermark.png";
            WatermarkTransparency = 5;
            WatermarkText = "";
            WatermarkFont = "Arial";
            WatermarkFontSize = 12;
        }
        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string AttachExtension { get; set; }
        /// <summary>
        /// 附件上传大小KB
        /// </summary>
        public int AttachSize { get; set; }
        /// <summary>
        /// 视频上传类型
        /// </summary>
        public string VideoExtension { get; set; }
        /// <summary>
        /// 视频上传大小KB
        /// </summary>
        public int VideoSize { get; set; }
        /// <summary>
        /// 图片上传类型
        /// </summary>
        public string ImgExtension { get; set; }
        /// <summary>
        /// 图片上传大小
        /// </summary>
        public int ImgSize { get; set; }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int ImgMaxHeight { get; set; }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int ImgMaxWidth { get; set; }
        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int ThumbnailHeight { get; set; }
        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int ThumbnailWidth { get; set; }
        /// <summary>
        /// 图片水印类型
        /// </summary>
        public int WatermarkType { get; set; }
        /// <summary>
        /// 图片水印位置
        /// </summary>
        public int WatermarkPosition { get; set; }
        /// <summary>
        /// 图片生成质量
        /// </summary>
        public int WatermarkimgQuality { get; set; }
        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string WatermarkPic { get; set; }
        /// <summary>
        /// 水印透明度
        /// </summary>
        public int WatermarkTransparency { get; set; }
        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText { get; set; }
        /// <summary>
        /// 文字字体
        /// </summary>
        public string WatermarkFont { get; set; }
        /// <summary>
        /// 文字大小(像素)
        /// </summary>
        public int WatermarkFontSize { get; set; }
    }
}
