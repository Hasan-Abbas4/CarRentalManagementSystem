using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class FavoriteCar
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public int UserId { get; set; }
        public DateTime AddedDate { get; set; }

        public virtual Car Car { get; set; }
    }
}