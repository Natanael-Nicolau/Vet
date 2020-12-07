using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor> GetDoctorByUserId(string userId);

        Task<Doctor> GetDoctorByUserEmail(string email);

        IQueryable<Doctor> GetAllWithUserAsync();
    }
}
