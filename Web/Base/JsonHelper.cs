using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web;
using System.Web.Mvc;
using X.Common;

namespace X.Web.Base
{
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();

            ContentType = "text/plain;charset=UTF-8";
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType =
                !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output)
                {
                    Formatting = Formatting
                };
                var utctimeconverter = new UTCDateTimeConverter();
                SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                SerializerSettings.Converters.Add(utctimeconverter);
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);

                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
    /// <summary>
    /// 定制自己的时间转换器
    /// </summary>
    public class UTCDateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                DateTime time = (DateTime)value;
                writer.WriteValue(time.ToLocalDate());
            }
            catch
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}