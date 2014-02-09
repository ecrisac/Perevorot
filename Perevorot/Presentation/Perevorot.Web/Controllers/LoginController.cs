using System.Web.Mvc;
using System.Web.Security;
using Perevorot.Domain.IServices.DomainInterfaces;
using Perevorot.Domain.Models.DomainEntities;
using Perevorot.Web.Filters;
using Perevorot.Web.Models;
using WebMatrix.WebData;

namespace Perevorot.Web.Controllers
{
    [InitializeSimpleMembership]
    public class LoginController : BaseController
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Get()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Get","Login");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Ошибка при регистрации");
            }

            return View(model);
        }

       
        [HttpPost]
        public JsonResult Login(LoginViewModel loginModel)
        {
            MembershipCreateStatus createStatus;
            if (ModelState.IsValid &&
                WebSecurity.Login(loginModel.UserName, loginModel.Password, persistCookie: loginModel.RememberMe))
            {
                Session.Add("User", loginModel.UserName);

                return Json(new { Result = "Success" });
            }
            return Json(new { Result = "Fail", Message = "Invalid user login" });
        }
    }
}