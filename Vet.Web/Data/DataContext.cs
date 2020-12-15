using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        [NotMapped]
        public TimeSpan OpeningHours => new TimeSpan(8, 0, 0);
        [NotMapped]
        public TimeSpan ClosingHours => new TimeSpan(20, 0, 0);


        public DbSet<Animal> Animals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<Specie> Species { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Speciality> Specialities { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Animal>()
                .Property(a => a.Weight)
                .HasColumnType("decimal(8,2)");

            //Disables Cascade
            var cascadeFKs = builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }


            base.OnModelCreating(builder);
        }
    }
}
