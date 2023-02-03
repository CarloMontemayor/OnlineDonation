using Microsoft.AspNetCore.Identity;
using OnlineDonation.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Data.Entity
{
    public class WebAppUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        [DisplayName("Signup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SignupDate { get; set; }

        [Required]
        [DisplayName("Gender")]
        public Gender Gender { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        public bool AllowLocation { get; set; }
        public bool AnsweredLocation { get; set; }
    }
}
