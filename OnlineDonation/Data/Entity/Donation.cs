using OnlineDonation.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Data.Entity
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Goal Amount")]
        public int GoalAmount { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Raised Money For")]
        public string RaisedMoneyFor { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "QR Image")]
        public string QRImagePath { get; set; }
        [DisplayName("Name")]
        public string UserId { get; set; }
        public WebAppUser User { get; set; }
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("ClosingDate")]
        public DateTime ClosingDate { get; set; }
        public Status Status { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string CustomImagePath { get; set; }
    }
}
