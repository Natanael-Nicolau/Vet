using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeByUserId(string userId)
        {
            return await _context.Employees
                .Include(e => e.User)
                .Where(e => e.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployeeByUserEmail(string email)
        {
            return await _context.Employees
                .Include(e => e.User)                
                .Where(e => e.User.Email == email)                
                .FirstOrDefaultAsync();
        }
    }
}
