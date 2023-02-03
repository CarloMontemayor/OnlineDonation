using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Data.Entity
{
    public class Visitor
    {
        [Key]
        public int VisitorId { get; set; }
        [Display(Name = "Visitor IP")]
        public string VisitorIp { get; set; }
        [Display(Name = "Date Visit")]
        public DateTime DateTime { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        public WebAppUser User { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }
        [Display(Name = "Longitude")]
        public string Longitude { get; set; }
    }
}
