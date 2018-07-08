
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace X.Constant
{
    /// <summary>
    /// 附件媒体类型
    /// </summary>
    public static class AttachmentType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        public const string Image = "Image";

        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        public const string Video = "Video";

        /// <summary>
        /// Flash
        /// </summary>
        [Description("Flash")]
        public const string Flash = "Flash";

        /// <summary>
        /// 音乐
        /// </summary>
        [Description("音乐")]
        public const string Audio = "Audio";

        /// <summary>
        /// 文档
        /// </summary>
        [Description("文档")]
        public const string Document = "Document";

        /// <summary>
        /// 压缩包
        /// </summary>
        [Description("压缩包")]
        public const string Compressed = "Compressed";

        /// <summary>
        /// 其他类型
        /// </summary>
        [Description("其他类型")]
        public const string Other = "Other";

    }
}
