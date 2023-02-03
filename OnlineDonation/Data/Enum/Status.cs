using System.ComponentModel.DataAnnotations;


namespace OnlineDonation.Data.Enum
{
    public enum Status
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Rejected")]
        Rejected
    }
}
