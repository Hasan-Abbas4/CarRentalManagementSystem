using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DamageReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }


        [Required]
        [Display(Name = "Damage Description")]
        public string Description { get; set; }

        [Display(Name = "Reported On")]
        public DateTime ReportDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } 

        [Display(Name = "Repair Cost (if any)")]
        public decimal? RepairCost { get; set; }

        [Display(Name = "Customer Charged")]
        public bool CustomerCharged { get; set; }
    }
}