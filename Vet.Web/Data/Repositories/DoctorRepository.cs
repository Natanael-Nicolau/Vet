using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly DataContext _context;

        public DoctorRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorByUserId(string userId)
        {
            return await _context.Doctors
                .Include(d => d.Employee)
                .ThenInclude(e => e.User)
                .Where(d => d.Employee.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Doctor> GetDoctorByUserEmail(string email)
        {
            return await _context.Doctors
                .Include(d => d.Employee)
                .ThenInclude(e => e.User)
                .Where(d => d.Employee.UserId == email)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public IQueryable<Doctor> GetAllWithUserAsync()
        {
            return _context.Doctors
                .Include(d => d.Employee)
                .ThenInclude(e => e.User)
                .AsNoTracking();
        }
    }
}
