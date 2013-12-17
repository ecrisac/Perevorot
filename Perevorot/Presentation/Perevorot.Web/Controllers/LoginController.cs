using System;
using Perevorot.Domain.IServices.DomainInterfaces;
using Perevorot.Domain.Models.Exceptions;
using Perevorot.Web.Models;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Web.Controllers
{
    using System.Web.Mvc;

    public class LoginController : BaseController
    {
        private readonly ILoginService _loginService;

        [HttpGet]
        public ActionResult Get()
        {
            return View("Login");
        }

        [HttpPost]
        public JsonResult Login(LoginModel loginModel)
        {
            try
            {
                var user = _loginService.GetUserByLoginData(
                              loginModel.UserName, loginModel.Password);
            }
            catch (FailedLoginException e)
            {
                return Json(new {Result="Fail", Message = e.Message});
            }

            Session.Add("User", loginModel.UserName);

            return Json(new { Result = "Success" });
        }

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
            
        }
    }
}