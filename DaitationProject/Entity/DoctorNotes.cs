using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class DoctorNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorNotesID { get; set; }
        [ForeignKey("appointments")]
        public int AppId { get; set; }

        public Appointments appointments { get; set; }
        public string Notes { get; set; }
    }
}