using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Car Make")]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Car Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Price Per Day (USD)")]
        [DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }

        [Display(Name = "Is Available?")]
        public bool IsAvailable { get; set; } = true;
        public virtual ICollection<Review> Reviews { get; set; }
        [NotMapped]
        public double AverageRating
        {
            get
            {
                return Reviews != null && Reviews.Any()
                    ? Math.Round(Reviews.Average(r => r.Rating), 1)
                    : 0;
            }
        }
        public virtual ICollection<FavoriteCar> FavoritedBy { get; set; }
    }
}