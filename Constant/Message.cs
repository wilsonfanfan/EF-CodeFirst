
namespace X.Constant
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public class ErrorCode
    {
        /// <summary>
        /// 说明
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
    }
    /// <summary>
    /// 错误消息
    /// </summary>
    public  class ErrorMessage
    {
        #region 数据操作错误
        /// <summary>
        /// 数据更新失败
        /// </summary>
        public static ErrorCode DataUpdateFailed = new ErrorCode { Code = 5001, Msg = "数据更新失败" };
        /// <summary>
        /// 数据删除失败
        /// </summary>
        public static ErrorCode DataDeleteFailed = new ErrorCode { Code = 5002, Msg = "数据删除失败" };
        /// <summary>
        /// 数据保存失败
        /// </summary>
        public static ErrorCode DataSaveFailed = new ErrorCode { Code = 5003, Msg = "保存过程中发生错误" };
        #endregion
        #region 不存在
        /// <summary>
        /// 数据不存在
        /// </summary>
        public static ErrorCode DataNotExist = new ErrorCode { Code = 4001, Msg = "数据不存在" };
        #endregion
    }
    /// <summary>
    /// 成功消息
    /// </summary>
    public static class SucceedMessage
    {
        /// <summary>
        /// 数据更新成功
        /// </summary>
        public const string DataUpdating = "数据更新成功";
    }

}
