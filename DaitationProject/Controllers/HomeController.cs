using DaitationProject.Abstract;
using DaitationProject.Common;
using DaitationProject.Concrete;
using DaitationProject.Entity;
using DaitationProject.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DaitationProject.Controllers
{
    public class HomeController : Controller
    {
        private IUser _UserRepo;
        private EFMessageRepository _msg = new EFMessageRepository();
        private EFDbContext db = new EFDbContext();
        public ActionResult Index(int id)
        {
            Payments payments = db.payments.Where(m => m.PatientID == id).OrderByDescending(q => q.PaymentID).FirstOrDefault();


            return View(payments);
        }
        public ActionResult Pay(int Id)
        {

            Payments payments = new Payments();
            var userModel = CommonFunctions.GetUserModel(Id);
            ViewBag.PatientID = userModel.UserID;
            db.payments.Add(payments).PatientID = userModel.UserID;
            db.payments.Add(payments).PaymentDate = System.DateTime.Now;
 

            //return View(appointments);
            return View(payments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pay(Payments payments)
        {
            if (ModelState.IsValid)
            {
               
                var userModel = CommonFunctions.GetUserModel(payments.PatientID);
                ViewBag.PatientID = userModel.UserID;
                db.payments.Add(payments).PatientID = userModel.UserID;
                db.payments.Add(payments).PaymentDate = System.DateTime.Now;

                db.SaveChanges();
                return View();
            }

            return View(payments);
        }


        [HttpPost]
        public ActionResult CreateCheckoutSession(string amount)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
        {
          new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount =Convert.ToInt32(amount)*100,
              Currency = "EGP",
                  //AED,USD,SAR,
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = "Price",
              },

            },
            Quantity = 1,
          },
        },
                Mode = "payment",
         
                SuccessUrl = "http://localhost:17495/Home/success",
                CancelUrl = "http://localhost:17495/Home/cancel",
            };
            StripeConfiguration.ApiKey = "sk_test_51LjfIkCFo9UCdezPAo6fElJ18kIPsu99CTYJXeis1GZ5P33ogVQahlzrtf7jIslX6oy5HGhw26tLzArbOTrdax0n00dIHa5Dx8";
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

          Response.Headers.Add("Location", session.Url);
            return new HttpStatusCodeResult(303);
        }
        [HttpPost]
        public ActionResult GetPayment()
        {
            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                Amount = 1099,
                SetupFutureUsage = "off_session",
                Currency = "EGP",
            };
            var paymentIntent = service.Create(options);
            return Json(paymentIntent);
        }

        public ActionResult success()
        {
            return View();
        }

        public ActionResult cancel()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }
     
        public ActionResult Sent()
        {
            return View();
        }
    }
}
