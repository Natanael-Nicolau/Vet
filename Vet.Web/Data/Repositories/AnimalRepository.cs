using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
    {
        private readonly DataContext _context;

        public AnimalRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Animal> GetClientAnimals(int ownerId)
        {
            return _context.Set<Animal>()
                .Include(a => a.Breed)
                .ThenInclude(b => b.Specie)
                .Where(a => a.Owner.Id == ownerId && !a.IsDeleted)
                .AsNoTracking();
        }
    }
}
