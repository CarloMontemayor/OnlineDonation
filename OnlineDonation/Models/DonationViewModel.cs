using Microsoft.AspNetCore.Http;
using OnlineDonation.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Models
{
    public class DonationViewModel
    {
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
        public IFormFile ImagePath { get; set; }
        public string ImagePathString { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "QR Image")]
        public IFormFile QRImagePath { get; set; }
        public string QRImagePathString { get; set; }


        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }


        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public IFormFile CustomFile { get; set; }
        public Status Status { get; set; }
    }
}
