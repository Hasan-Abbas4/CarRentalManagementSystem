using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        [Required]
        public string Status { get; set; } = "Pending";

        public int RentalDays => (EndDate - StartDate).Days;
        public decimal TotalAmount => Car != null ? RentalDays * Car.PricePerDay : 0;
        public string PaymentStatus { get; set; } = "Pending";
        [Display(Name = "Need Driver?")]
        public bool NeedsDriver { get; set; } = false;

        public int? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string CouponCode { get; set; }
        public virtual ICollection<DamageReport> DamageReports { get; set; }

    }
}