using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaitationProject.Entity
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }
        [Required]
        public string Role { get; set; }
    }
}