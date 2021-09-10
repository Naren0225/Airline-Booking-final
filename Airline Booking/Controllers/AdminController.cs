using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Airline_Booking.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(user user)
        {
            if (user.name.Equals("admin") && user.password.Equals("admin"))
            {
                return RedirectToAction("Index", "Flights");
            }

            ModelState.AddModelError("", "Incorrect Username And Password");
            return View();
        }
    }
}