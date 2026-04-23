using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ContactNumber { get; set; }

        public string LicenseNumber { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}