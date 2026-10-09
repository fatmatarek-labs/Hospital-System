using Hospital_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Doctor> Doctors { get; set; }
        public DbSet<Models.Appointment> Appointments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source = FATMA\\SQLEXPRESS; initial catalog = Hospital; Integrated Security = True; Connect Timeout = 30; Encrypt = True; Trust Server Certificate = True; Application Intent = ReadWrite; Multi Subnet Failover = False; Command Timeout = 30");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //عشان ميحجزش اكتر من ميعاد لنفس الدكتور في نفس اليوم ونفس الوقت :)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new
                {
                    a.DoctorId,
                    a.AppointmentDate,
                    a.AppointmentTime
                })
                .IsUnique();
        }
    }
}
    