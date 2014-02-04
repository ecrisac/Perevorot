namespace Perevorot.Web.Controllers
{
    using NLog;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet, Authorize(Users = "Harry")]
        public ActionResult Index()
        {
            logger.Log(LogLevel.Info, "User logged in, user: {0}", "Harry Potter");
            return View("Index");
        }
        

    }
}