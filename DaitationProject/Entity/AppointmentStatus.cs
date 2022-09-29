using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class AppointmentStatus
    {
        [Key]
        public int AppointmentStatusID { get; set; }
        public string Status { get; set; }

    }
}
