using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Airline_Booking.Controllers
{
    public class HomeController : Controller
    {
        private airlineEntities db = new airlineEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Book()
        {
            return View(db.flights.ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            flight flight = db.flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }
        [Authorize]
        public ActionResult MyOrders()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            var bookings = db.bookings.Include(b => b.flight).Include(b => b.user).Where(u => u.user.id == id);
            return View(bookings.ToList());
        }

        [HttpGet]
        [Authorize]
        public ActionResult MakeOrder(int? fid)
        {
            int uid = Convert.ToInt32(Session["UserID"]);

            if (fid == null && uid == null)
            {
                return HttpNotFound();
            }
            else
            {
                booking order = new booking();
                order.uid = uid;
                order.fid = fid;
                order.qty = 1;
                db.bookings.Add(order);
                db.SaveChanges();
                return RedirectToAction("MyOrders");
            }
        }

    }
}