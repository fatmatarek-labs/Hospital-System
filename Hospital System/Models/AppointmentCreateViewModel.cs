using System.ComponentModel.DataAnnotations;

namespace Hospital_System.Models
{
    public class AppointmentCreateViewModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string AppointmentTime { get; set; } = string.Empty;
    }
}
