using DaitationProject.Concrete;
using DaitationProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaitationProject.Controllers
{
    public class videocallController : Controller
    {
        //
        // GET: /videocall/
        //private EFDbContext db = new EFDbContext();
        //public ActionResult Index(int id)
        //{
        //    var userModel = CommonFunctions.GetUserModel(id);
        //    return View(userModel);
        //}

        public ActionResult Index()
        {
            return View();
        }

    }
}
