using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime DatePosted { get; set; }
    }
}