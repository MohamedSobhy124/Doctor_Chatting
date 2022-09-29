using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DaitationProject.Common
{
    public class MySession : IReadOnlySessionState 
    {
        private MySession()
        {
            UserID = 0;
            RoleID = 0;
            Name = "";
            Email = "";
            ProfilePicture = "";
            Gender = "";
        }

        // Gets the current session.
        public static MySession Current
        {
            get
            {
                MySession session =
                  (MySession)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new MySession();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }

      
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Gender { get; set; }
    }
}