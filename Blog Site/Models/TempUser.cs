using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Blog_Site.Models
{
    public class TempUser
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}