using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class DiagnosisTypes
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
    }
}