using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppointmentScheduler.Domain.Models;

namespace AppointmentScheduler.Infrastructure.Data
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options)
            : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderAvailability> ProviderAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configure relationships and constraints
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany()
                .HasForeignKey(a => a.ProviderId);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.ProviderId, a.AppointmentDateTime })
                .IsUnique();

            modelBuilder.Entity<Provider>()
               .HasMany(p => p.Availabilities)
               .WithOne(pa => pa.Provider)
               .HasForeignKey(pa => pa.ProviderId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Provider>()
             .HasMany(p => p.Appointments)
             .WithOne(a => a.Provider)
             .HasForeignKey(a => a.ProviderId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
