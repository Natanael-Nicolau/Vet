using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;
using Vet.Web.Models.Appointments;

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


        public async Task<List<Appointment>> GetAllClient(int id)
        {
            return await _context.Set<Appointment>()
                    .Where(a => a.Animal.Owner.Id == id && a.IsAproved && !a.IsDeleted)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                    .Include(a => a.Animal)
                    .ThenInclude(a => a.Owner.User)
                    .ToListAsync();
        }

        public async Task<List<Appointment>> GetAllDoctor(int id)
        {
            return await _context.Set<Appointment>()
                    .Where(a => a.Doctor.Id == id && a.IsAproved && !a.IsDeleted)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                    .Include(a => a.Animal)
                    .ThenInclude(a => a.Owner.User)
                    .ToListAsync();
        }

        public async Task<List<Appointment>> GetNotAprovedDoctor(int id)
        {
            return await _context.Appointments
                    .Where(a => a.DoctorId == id && !a.IsAproved)
                    .Include(a => a.Doctor)
                        .ThenInclude(d => d.User)
                    .Include(a => a.Animal)
                        .ThenInclude(a => a.Owner.User)
                        .ToListAsync();
        }

        public async Task<List<Appointment>> GetAllAdmin()
        {
            return await _context.Set<Appointment>()
                    .Where(a => a.IsAproved && !a.IsDeleted)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                    .Include(a => a.Animal)
                    .ThenInclude(a => a.Owner.User)
                    .ToListAsync();
        }

        public async Task<Appointment> GetAppointmentWithAllAsync(int id)
        {
            return await _context.Appointments
                .Where(a => a.Id == id)
                .Include(a => a.Animal)
                    .ThenInclude(a => a.Owner)
                        .ThenInclude(c => c.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Room)
                        .ThenInclude(r => r.Speciality)
                .FirstOrDefaultAsync();
                


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

        public new async Task DeleteAsync(Appointment appointment)
        {
            _context.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
