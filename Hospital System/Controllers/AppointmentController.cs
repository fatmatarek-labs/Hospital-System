using Hospital_System.Data;
using Hospital_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);

            if (doctor == null)
                return NotFound();

            var model = new AppointmentCreateViewModel
            {
                DoctorId = doctor.Id,
                DoctorName = doctor.Name,
                Specialization = doctor.Specialization,
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = "09:00"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            AppointmentCreateViewModel model)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id == model.DoctorId);

            if (doctor == null)
                return NotFound();

            model.DoctorName = doctor.Name;
            model.Specialization = doctor.Specialization;

            if (ModelState.IsValid)
            {
                var date = model.AppointmentDate.Date;
                var time = model.AppointmentTime;

                if (date.Date < DateTime.Today)
                {
                    ModelState.AddModelError(
                        nameof(model.AppointmentDate),
                        "Please select today or a future date.");
                }

                if (date.DayOfWeek == DayOfWeek.Friday ||
                    date.DayOfWeek == DayOfWeek.Saturday)
                {
                    ModelState.AddModelError(
                        nameof(model.AppointmentDate),
                        "Appointments are available Sunday through Thursday.");
                }

                var validTimes = Enumerable.Range(0, 16)
                    .Select(i => DateTime.Today.AddHours(9).AddMinutes(i * 30).ToString("HH:mm"))
                    .ToList();

                validTimes.Add("16:30");

                if (!validTimes.Contains(time))
                {
                    ModelState.AddModelError(
                        nameof(model.AppointmentTime),
                        "Please select a valid time.");
                }

                bool alreadyBooked = await _context.Appointments.AnyAsync(a =>
                    a.DoctorId == model.DoctorId &&
                    a.AppointmentDate == date &&
                    a.AppointmentTime == time);

                if (alreadyBooked)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "This doctor is already booked at that time.");
                }
            }

            if (!ModelState.IsValid)
                return View(model);

            var appointment = new Appointment
            {
                PatientName = model.PatientName,
                AppointmentDate = model.AppointmentDate.Date,
                AppointmentTime = model.AppointmentTime,
                DoctorId = model.DoctorId
            };

            _context.Appointments.Add(appointment);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This time was just booked. Please choose another.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();

            return View(appointments);
        }
    }
}