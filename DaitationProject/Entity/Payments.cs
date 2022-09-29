using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class Payments
    {
        [Key]
        public int PaymentID { get; set; }
        public Double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public User Patient { get; set; }
    }
}