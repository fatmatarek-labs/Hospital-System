using Hospital_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? name,
            string? specialization,
            int page = 1)
        {
            const int pageSize = 4;

            var query = _context.Doctors.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(specialization))
                query = query.Where(d =>
                    d.Specialization == specialization);

            var totalDoctors = await query.CountAsync();

            var doctors = await query
                .OrderBy(d => d.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages =
                (int)Math.Ceiling(totalDoctors / (double)pageSize);

            ViewBag.Name = name;
            ViewBag.Specialization = specialization;

            ViewBag.Specializations = await _context.Doctors
                .Select(d => d.Specialization)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            return View(doctors);
        }
    }
}