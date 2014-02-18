using System.Text;
using System.Web.Mvc;
using Perevorot.Web.Configuration;
using Perevorot.Web.Filters;

namespace Perevorot.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType,
            Encoding contentEncoding, JsonRequestBehavior behavior) 
        {
            var jsonNetResult = new JsonNetResult(data)
                {
                    ContentType = contentType,
                    ContentEncoding = contentEncoding,
                    JsonRequestBehavior = behavior
                };

            return jsonNetResult;
        }
    }
}