using System.ComponentModel.DataAnnotations;

namespace Hospital_System.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Specialization { get; set; } 

        public string ImageUrl { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
