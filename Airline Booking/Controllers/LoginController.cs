using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Airline_Booking.Controllers
{
    public class LoginController : Controller
    {
        private airlineEntities db = new airlineEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(user model, string returnUrl)
        {
            bool isValid = db.users.Any(u => u.email.Equals(model.email) && u.password.Equals(model.password));

            if (isValid)
            {
                var obj = db.users.Where(a => a.email.Equals(model.email) && a.password.Equals(model.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.id.ToString();
                    Session["UserName"] = obj.email.ToString();
                }
                FormsAuthentication.SetAuthCookie(model.email, false);
                // Info.    
                return this.RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Incorrect Username And Password");
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(user user)
        {
            if (ModelState.IsValid)
            {
                
                db.users.Add(user);
                db.SaveChanges();
                ModelState.AddModelError("", "User Account Created Successfully!!");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Unable to create account!!");
            return View();
        }



        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.    
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.    
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Index", "Home");
        }
    }
}