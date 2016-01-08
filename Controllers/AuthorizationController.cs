using System;
using System.Web.Mvc;
using System.Web.Security;
using KitchenHelperServer.Models;

namespace KitchenHelperServer.Controllers
{
    [AllowAnonymous]
    public class AuthorizationController : Controller
    {
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignIn(UserGroup model)
        {
            try
            {
                if (!Membership.ValidateUser(model.Name, model.Password))
                {
                    return RedirectToAction("SignIn", "Authorization");
                }

                FormsAuthentication.SetAuthCookie(model.Name, false);
                return RedirectToAction("Index", "Chat");
            }
            catch (Exception exception)
            {
                return View(model);
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
    }
}