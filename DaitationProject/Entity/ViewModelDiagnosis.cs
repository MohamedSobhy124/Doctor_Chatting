using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class ViewModelDiagnosis
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("diagnosisList")]
        public int DiagnosisID { get; set; }
        public DiagnosisList diagnosisList { get; set; }
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
     
        public string Type { get; set; }
        public int Appid { get; set; }
    }
}