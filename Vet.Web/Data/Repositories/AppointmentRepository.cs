using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DataContext _context;

        public AppointmentRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Appointment> GetAllAppointmentsDay()
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Start.Value.Date == DateTime.UtcNow.Date)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetAllClient(int clientId)
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Animal.Owner.Id == clientId)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Employee.User)
                    .Include(a => a.Animal)
                    .ThenInclude(a => a.Owner.User)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetDayAppointmentsForClient(int clientId)
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Animal.Owner.Id == clientId)
                    .Where(a => a.Start.Value.Date == DateTime.UtcNow.Date)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetDoctorAppointmentsDay(int doctorId)
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Doctor.Id == doctorId)
                    .Where(a => a.Start.Value.Date == DateTime.UtcNow.Date)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetMonthAppointmentsForAll()
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Start.Value.Month == DateTime.UtcNow.Month)
                    .Where(a => a.Start.Value.Year == DateTime.UtcNow.Year)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetMonthAppointmentsForClient(int clientId)
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Animal.Owner.Id  == clientId)
                    .Where(a => a.Start.Value.Month == DateTime.UtcNow.Month)
                    .Where(a => a.Start.Value.Year == DateTime.UtcNow.Year)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetMonthAppointmentsForDoctor(int doctorId)
        {
            return _context.Set<Appointment>()
                    .Where(a => a.Doctor.Id == doctorId)
                    .Where(a => a.Start.Value.Month == DateTime.UtcNow.Month)
                    .Where(a => a.Start.Value.Year == DateTime.UtcNow.Year)
                    .AsNoTracking();
        }

        public IQueryable<Appointment> GetUnavailableAppointments(DateTime day, int roomId)
        {
            return _context.Appointments
                 .Include(a => a.Doctor)
                 .Where(a => a.Start.Value.Date == day.Date)
                 .Where(a => a.Doctor.RoomId == roomId)                 
                 .AsNoTracking();
        }
    }
}
