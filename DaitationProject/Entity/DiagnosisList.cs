using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class DiagnosisList
    {
        [Key]
        public int DiagnosisID { get; set; }
        public string DCode { get; set; }
        public string DDescription { get; set; }
        [NotMapped]
        public string DisplayDiagnosis
        {
            get
            {
                return DCode + "  " + DDescription;
            }
        }
    }
}