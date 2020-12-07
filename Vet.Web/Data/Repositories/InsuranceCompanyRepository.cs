using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class InsuranceCompanyRepository : GenericRepository<InsuranceCompany>, IInsuranceCompanyRepository
    {
        private readonly DataContext _context;

        public InsuranceCompanyRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
