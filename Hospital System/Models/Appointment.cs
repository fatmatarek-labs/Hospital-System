using System.ComponentModel.DataAnnotations;

namespace Hospital_System.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string AppointmentTime { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
