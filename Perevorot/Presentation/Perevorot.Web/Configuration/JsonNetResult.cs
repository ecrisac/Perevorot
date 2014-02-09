using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Perevorot.Web.Configuration
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult(object data)
        {
            if (data == null) 
                throw new ArgumentNullException("data");
            Data = data;
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            Formatting = HttpContext.Current.IsDebuggingEnabled ? Formatting.Indented : Formatting.None;
        }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Method is not allowed.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) {Formatting = Formatting};
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}

