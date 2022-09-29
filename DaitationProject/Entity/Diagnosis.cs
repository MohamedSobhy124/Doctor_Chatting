using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class Diagnosis
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("diagnosisList")]
        public int DiagnosisID { get; set; }
        public string DCode { get; set; }
        public string DDescription { get; set; }
        public string Type { get; set; }
        public int Appid { get; set; }
        public  DiagnosisList  diagnosisList { get; set; }


    }
}