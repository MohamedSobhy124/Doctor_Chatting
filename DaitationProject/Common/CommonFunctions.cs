using DaitationProject.Concrete;

using DaitationProject.Entity;
using DaitationProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DaitationProject
{
    public static class CommonFunctions
    {
        public static string GetProfilePicture(string profilePicture, string gender)
        {
            string profilePicturePath = "";
            if (string.IsNullOrEmpty(profilePicture))
            {
                if (gender == "Female")
                {
                    profilePicturePath = "/Content/Images/female-default-pic.jpg";
                }
                else
                {
                    profilePicturePath = "/Content/Images/male-default-pic.jpg";
                }
            }
            else
            {
                profilePicturePath = profilePicture;
            }
            return profilePicturePath;
        }
        public static UserModel GetUserModel(int id, User objentity = null, string friendRequestStatus = "", bool isRequestReceived = false)
        {
            var user = new User();
            if (objentity != null)
            {
                user = objentity;
            }
            else
            {
                EFUserRepository _UserRepo = new EFUserRepository();
                user = _UserRepo.GetUserById(id);
            }
            UserModel objmodel = new UserModel();
            if (user != null)
            {
                objmodel.IsRequestReceived = isRequestReceived;
                objmodel.FriendRequestStatus = friendRequestStatus;
                objmodel.UserID = user.UserID;
                objmodel.RoleID = user.RoleID;
                objmodel.Name = user.Name;
                objmodel.UserName = user.UserName;
                objmodel.Password = user.Password;
                objmodel.address = user.address;
                objmodel.Email = user.Email;
                objmodel.Mobile = user.Mobile;
                objmodel.ProfilePicture = CommonFunctions.GetProfilePicture(user.ProfilePicture, user.Gender);
                objmodel.Gender = user.Gender;
                objmodel.DOB = user.DOB.ToShortDateString();
                if (user.DOB != null)
                {
                    objmodel.Age = Convert.ToString(Math.Floor(DateTime.Now.Subtract(Convert.ToDateTime(user.DOB)).TotalDays / 365.0)) + " Years";
                }
                else
                {
                    objmodel.Age = "NaN";
                }
                objmodel.Bio = user.Bio;
            }
            return objmodel;
        }
        public static MessageModel GetMessageModel(ChatMessage objentity)
        {
            MessageModel objmodel = new MessageModel();
            objmodel.ChatMessageID = objentity.ChatMessageID;
            objmodel.FromUserID = objentity.FromUserID;
            objmodel.ToUserID = objentity.ToUserID;
            objmodel.Message = objentity.Message;
            objmodel.Status = objentity.Status;
            objmodel.CreatedOn =Convert.ToString(objentity.CreatedOn);
            objmodel.UpdatedOn = Convert.ToString(objentity.UpdatedOn);
            objmodel.ViewedOn = Convert.ToString(objentity.ViewedOn);
            objmodel.IsActive = objentity.IsActive;
            return objmodel;
        }
        public static List<DiagnosisList> LoadData()
        {
            List<DiagnosisList> lst = new List<DiagnosisList>();

            try
            {
                string line = string.Empty;
                string srcFilePath = "Content/files/country_list.txt";
                var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                var fullPath = Path.Combine(rootPath, srcFilePath);
                string filePath = new Uri(fullPath).LocalPath;
                StreamReader src = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read));

                // while to read the file
                while ((line = src.ReadLine()) != null)
                {
                    DiagnosisList infoLst = new DiagnosisList();
                    string[] info = line.Split(',');

                    ////Setting
                    //infoLst.DCode = Convert.ToInt32(info[0].ToString());
                    //infoLst.Country_Name = info[1].ToString();

                    lst.Add(infoLst);
                }
                src.Dispose();
                src.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }
    }
}