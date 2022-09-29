using DaitationProject.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Models
{
    public class ViewModelSendEmail
    {
        public string From
        {
            get;
            set;
        }
        public string To
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
        public int AppID { get; set; }
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AppDate { get; set; } = DateTime.Now;

        //[DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh: mm tt}")]
        public DateTime AppTime { get; set; } = DateTime.Now;

        //public User PatientID { get; set; }
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public User Patient { get; set; }
        public string PatientName { get; set; }
        public string Email { get; set; }
        [ForeignKey("AppStatus")]
        public int StatusID { get; set; }
        public virtual AppointmentStatus AppStatus { get; set; }

        public string description { get; set; }
    }
}