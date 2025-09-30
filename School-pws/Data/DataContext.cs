using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School_pws.Data.Entities;
using System.Net.Sockets;
using System.Numerics;

namespace School_pws.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ApplicationDetails> ApplicationDetails { get; set; }

        public DbSet<ApplicationDetailsTemp> ApplicationDetailsTemp { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subject>()
                .HasOne(s => s.User)
                .WithMany()      
                .IsRequired()           
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
