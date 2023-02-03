using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Data.Entity
{
    public class Share
    {
        [Key]
        public int ShareId { get; set; }
        [Display(Name = "Date Shared")]
        public DateTime DateTime { get; set; }
        [DisplayName("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
    }
}
