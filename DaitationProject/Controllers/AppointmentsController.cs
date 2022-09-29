using DaitationProject.Abstract;
using DaitationProject.Common;
using DaitationProject.Concrete;
using DaitationProject.Entity;
using DaitationProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DaitationProject.Controllers
{
    public class AppointmentsController : Controller
    {

        private IUser _UserRepo;
        private EFMessageRepository _msg = new EFMessageRepository();
        private EFDbContext db = new EFDbContext();
        // GET: /Appointments/
        private AppointmentStatus appst = new AppointmentStatus();
        public ActionResult Index()
        {
        
            var app = db.Appointments.Include("AppStatus").ToList().OrderBy(m=>m.AppDate);
            ViewBag._Status = new SelectList(db.appointmentStatuses, "AppointmentStatusID", "Status");
            return View(app);
        }
        public ActionResult PatientAppointments(int id)
        {

            List<Appointments> appointments = db.Appointments.Where(s => s.PatientID ==id).OrderBy(s => s.AppDate).ToList();
            return View(appointments);
        }

        public ActionResult Create()
        {
            Appointments appointments = new Appointments();

            var userModel = CommonFunctions.GetUserModel(MySession.Current.UserID);

            db.Appointments.Add(appointments).PatientID = MySession.Current.UserID;
            db.Appointments.Add(appointments).PatientName = MySession.Current.Name;
            db.Appointments.Add(appointments).AppTime = DateTime.Now;
            db.Appointments.Add(appointments).Email = userModel.Email;

            return View(appointments);

     
        }

        //private IUser _UserRepo;
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppID,AppDate,AppTime,StatusID,description,PatientID")] Appointments appointments)
        {
            if (ModelState.IsValid)
            {
                // Add App
                var userModel = CommonFunctions.GetUserModel(MySession.Current.UserID);
                db.Appointments.Add(appointments).PatientID = MySession.Current.UserID;
                db.Appointments.Add(appointments).PatientName = MySession.Current.Name;
                db.Appointments.Add(appointments).Email = userModel.Email;
                db.Appointments.Add(appointments).StatusID=1;
                db.Appointments.Add(appointments);

                //Send notification
                UserNotification userNotification = new UserNotification();
                db.UserNotifications.Add(userNotification).FromUserID= MySession.Current.UserID;
                db.UserNotifications.Add(userNotification).ToUserID = 1;
                db.UserNotifications.Add(userNotification).Status = "New";
                db.UserNotifications.Add(userNotification).IsActive = true;
                db.UserNotifications.Add(userNotification).NotificationType = "New Appointment";
                db.UserNotifications.Add(userNotification).UpdatedOn = DateTime.Now;
                db.UserNotifications.Add(userNotification).CreatedOn = DateTime.Now;



                db.SaveChanges();
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add("smosobhy@gmail.com");
                    mail.From = new MailAddress("smosobhy@gmail.com");
                    mail.Subject = "Appointment Conformation";
                    string Body = ("New Appointment for : " + appointments.PatientName + " - Apointment Day : " + appointments.AppDate.ToString("dddd, dd MMMM yyyy") + " at : " + appointments.AppTime.ToString("hh:mm tt") + "  " + appointments.description)+" - Email : "+appointments.Email;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("smosobhy@gmail.com", "ddkpwjbsmxpnzlbb"); // Enter seders User name and password  
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return RedirectToAction("Chat", "User");
                }
                catch { return RedirectToAction("Chat", "User"); }
            }

            return RedirectToAction("Chat", "User");
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            var App = appointments.AppID;

            DoctorNotes doctorNotes = new DoctorNotes();
            if (db.doctorNotes.Any(a => a.AppId == appointments.AppID))
            {
                doctorNotes.AppId = db.doctorNotes.FirstOrDefault(a => a.AppId == appointments.AppID).AppId;
                doctorNotes.Notes = db.doctorNotes.FirstOrDefault(s => s.AppId == id).Notes;
                doctorNotes.DoctorNotesID = db.doctorNotes.FirstOrDefault(s => s.AppId == id).DoctorNotesID;

                return View(doctorNotes);
            }
            else
            {
                doctorNotes.AppId = App;
                return View(doctorNotes);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(DoctorNotes doctorNotes)
        {
            var doc = db.doctorNotes.Any(a => a.DoctorNotesID == doctorNotes.DoctorNotesID);
         
            if (doc ==false)
            {
                db.Entry(doctorNotes).State = EntityState.Added;
            }
            else
            {
                db.Entry(doctorNotes).State = EntityState.Modified;

            }

            db.SaveChanges();
            TempData["Success"] = "Success message text.";
            return View(doctorNotes);
        }
        [HttpGet]
        public ActionResult Diagnosis(int id)
        {

            Appointments appointments = db.Appointments.FirstOrDefault(a => a.AppID == id);
           // dynamic mymodel = new ExpandoObject();
            var App = appointments.AppID;
            Diagnosis diagnosis = new Entity.Diagnosis();
            diagnosis.Appid = App;
            ViewBag.DCode = new SelectList(db.DiagnosisList, "DCode", "DisplayDiagnosis");
        
            ViewBag.DiagnosisID = new SelectList(db.DiagnosisTypes, "DiagnosisID", "DiagnosisID");
            ViewBag.getDiagnosis = db.diagnoses.Where(d => d.Appid == id).ToList();

            return View(diagnosis);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Diagnosis(Diagnosis diagnosis)
        {
            ViewBag.getDiagnosis = db.diagnoses.Where(d => d.Appid == diagnosis.Appid).ToList();
            ViewBag.DCode = new SelectList(db.DiagnosisList, "DCode", "DisplayDiagnosis");
      
            if (db.diagnoses.Any(a=>a.Appid==diagnosis.Appid))
            {
                db.diagnoses.Add(diagnosis).Type = "Secondary";
            }
            else
            {
                db.diagnoses.Add(diagnosis).Type = "Principal";
            }
         
            db.diagnoses.Add(diagnosis).DDescription = db.DiagnosisList.FirstOrDefault(a => a.DCode == diagnosis.DCode).DDescription;
            db.diagnoses.Add(diagnosis).DiagnosisID = db.DiagnosisList.FirstOrDefault(a => a.DCode == diagnosis.DCode).DiagnosisID;
            db.Entry(diagnosis).State = EntityState.Added;
     



            db.SaveChanges();

            return RedirectToAction("Diagnosis");
            //return Json(diagnosis, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Details(DoctorNotes doctorNotes)
        //{
        //    //var userModel = CommonFunctions.GetUserModel(MySession.Current.UserID);


        //    db.doctorNotes.Add(doctorNotes).AppId = doctorNotes.AppId;
        //    db.doctorNotes.Add(doctorNotes).appointments = doctorNotes.appointments;
        //    db.doctorNotes.Add(doctorNotes).Notes = doctorNotes.Notes;
        //    db.doctorNotes.Add(doctorNotes).DoctorNotesID = doctorNotes.DoctorNotesID;


        //    //db.Entry(doctorNotes).State = EntityState.Modified;

        //    db.SaveChanges();

        //    return View();
        //}
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);

            if (appointments == null)
            {
                return HttpNotFound();
            }
            ViewBag._Status = new SelectList(db.appointmentStatuses, "AppointmentStatusID", "Status");

            return View(appointments);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppID,AppDate,AppTime,StatusID,description,PatientID,PatientName,Email")] Appointments appointments)
        {
            if (ModelState.IsValid)
            {
              
                db.Entry(appointments).State = EntityState.Modified;

                db.SaveChanges();
             
                ChatMessage chatMessage = new ChatMessage();
                if (chatMessage.ToUserID != MySession.Current.UserID)
                {
                    chatMessage.CreatedOn = System.DateTime.Now;
                    chatMessage.FromUserID = MySession.Current.UserID;
                    chatMessage.IsActive = true;
                    chatMessage.Message = ("Dear " + appointments.PatientName + " This Conformation for your Apointment Day : " + appointments.AppDate.ToString("dddd, dd MMMM yyyy") + " at : " + appointments.AppTime.ToString("hh:mm tt") + "  " + appointments.description);
                    chatMessage.ToUserID = appointments.PatientID;

                    chatMessage.UpdatedOn = System.DateTime.Now;
                    chatMessage.ViewedOn = System.DateTime.Now;
                    chatMessage.Status = "Sent";
                    _msg.SaveChatMessage(chatMessage);
                    try
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(appointments.Email);
                        mail.From = new MailAddress("smosobhy@gmail.com");
                        mail.Subject = "Appointment Conformation";
                        string Body = ("Dear " + appointments.PatientName + " This Conformation for your Apointment Day : " + appointments.AppDate.ToString("dddd, dd MMMM yyyy") + " at : " + appointments.AppTime.ToString("hh:mm tt") + "  " + appointments.description);
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("smosobhy@gmail.com", "ddkpwjbsmxpnzlbb"); // Enter seders User name and password  
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                        return RedirectToAction("Index");
                    }
                    catch { return RedirectToAction("Index"); }
                }
                else
                    return RedirectToAction("Index");


            }



           



            return View(appointments);
        }
        public ActionResult Delete(int? id)
        {

            Appointments appointments = db.Appointments.Find(id);
            db.Appointments.Remove(appointments);
            db.SaveChanges();
            return RedirectToAction("Index");
         
        }
        public ActionResult DeleteDiagnosis(int? id)
        {
           
            Diagnosis diagnosis = db.diagnoses.Find(id);
            db.diagnoses.Remove(diagnosis);
            db.SaveChanges();
            ViewBag.getDiagnosis = db.diagnoses.Where(d => d.Appid == diagnosis.Appid).ToList();
            ViewBag.DCode = new SelectList(db.DiagnosisList, "DCode", "DisplayDiagnosis");

            return View("Diagnosis",diagnosis );

        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            //appointments.co
            
            db.Appointments.Remove(appointments);
       
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
        public ActionResult SendEmail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);

            if (appointments == null)
            {
                return HttpNotFound();
            }
            ViewBag._Email = appointments.Email;

            MailModel mailModel = new MailModel();
            mailModel.From = "smosobhy@gmail.com";
            mailModel.To = appointments.Email;
           
            return View(mailModel);
        }

        [HttpPost]
        public ActionResult SendEmail(MailModel _objModelMail)
        {

            ViewBag._From = new SelectList(db.Appointments, "Email", "Email");
            try
            {

                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress("smosobhy@gmail.com");
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("smosobhy@gmail.com", "ddkpwjbsmxpnzlbb"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);


                ViewBag.SuccessMsg = "email successfully sent";
                return View();
            }
                catch {
                return View();
                     }
          }

    }
}
