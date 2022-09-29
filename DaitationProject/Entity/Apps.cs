using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DaitationProject.Entity
{
    public class Apps
    {
      [key]
        public int  ID { get; set; }
        [DataType(DataType.Time)]
        public DateTime StTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; } 
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public User Patient { get; set; }
    }
}