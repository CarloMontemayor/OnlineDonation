using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Data.Entity
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        [Display(Name = "Amount")]
        public int Amount { get; set; }
        [Display(Name = "Reference Code")]
        public long ReferenceCode { get; set; }
        [Display(Name = "Date Donated")]
        public DateTime DateTime { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        [DisplayName("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
        public WebAppUser User { get; set; }
    }
}
