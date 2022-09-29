using DaitationProject.Abstract;
using DaitationProject.Common;
using DaitationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaitationProject.Controllers
{
    public class ChatController : Controller
    {
        private IUser _UserRepo;
        private IMessage _MessageRepo;
        public ChatController(IUser UserRepo, IMessage MessageRepo)
        {
            this._UserRepo = UserRepo;
            this._MessageRepo = MessageRepo;
        }
      
            public ActionResult _Messages(int Id)
        {
            var userModel = CommonFunctions.GetUserModel(Id);
            var messages = _MessageRepo.GetChatMessagesByUserID(MySession.Current.UserID, Id);
            var objmodel = new ChatMessageModel();
            objmodel.UserDetail = userModel;
            ViewBag.userid = userModel;
           
            objmodel.ChatMessages = messages.Messages.Select(m => CommonFunctions.GetMessageModel(m)).ToList();
            objmodel.LastChatMessageId = messages.LastChatMessageId;
            var onlineStatus = _UserRepo.GetUserOnlineStatus(Id);
            if (onlineStatus != null)
            {
                objmodel.IsOnline = onlineStatus.IsOnline;
                objmodel.UserDetail.UserID = onlineStatus.UserID;
                objmodel.LastSeen = Convert.ToString(onlineStatus.LastUpdationTime);
            }
            return View(objmodel);
        }

        
        public ActionResult GetRecentMessages(int Id, int lastChatMessageId)
        {
            var messages = _MessageRepo.GetChatMessagesByUserID(MySession.Current.UserID, Id, lastChatMessageId);
            var objmodel = new ChatMessageModel();
            objmodel.ChatMessages = messages.Messages.Select(m => CommonFunctions.GetMessageModel(m)).ToList();
            objmodel.LastChatMessageId = messages.LastChatMessageId;
            return Json(objmodel, JsonRequestBehavior.AllowGet);
        }
    }
}
