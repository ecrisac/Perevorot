namespace Perevorot.Web.Controllers
{
    using NLog;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet, CustomAutorizeAttribute(Roles = "Operators")]
        public ActionResult Index()
        {
            logger.Log(LogLevel.Info, "User logged in, user: {0}", "Harry Potter");
            return View("Index");
        }

    }
}