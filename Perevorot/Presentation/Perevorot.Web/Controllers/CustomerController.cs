namespace Perevorot.Web.Controllers
{
    using System.Web.Mvc;

    public class CustomerController : BaseController
    {
        public CustomerController()
        {
        }

        [HttpGet]
        public JsonResult Index()
        {
            return Json("super");
        }

    }
}