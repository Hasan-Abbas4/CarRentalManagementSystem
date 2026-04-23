using System.Data.Entity;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext() : base("CarRentalDBContext") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DamageReport> DamageReports { get; set; }
        public DbSet<Coupon>Coupons { get; set; }
        public DbSet<FavoriteCar> FavoriteCars { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Review>()
                .HasRequired(r => r.Car)
                .WithMany()
                .HasForeignKey(r => r.CarId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Review>()
                .HasRequired(r => r.Booking)
                .WithMany()
                .HasForeignKey(r => r.BookingId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Review>()
                .HasRequired(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DamageReport>()
            .HasRequired(d => d.Booking)
            .WithMany(b => b.DamageReports)
            .HasForeignKey(d => d.BookingId);

            modelBuilder.Entity<Booking>()
        .HasOptional(b => b.Driver) 
        .WithMany() 
        .HasForeignKey(b => b.DriverId)
        .WillCascadeOnDelete(true); 

       
            modelBuilder.Entity<Review>()
                .HasRequired(r => r.User)  
                .WithMany()  
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(true);

        }

    }
}