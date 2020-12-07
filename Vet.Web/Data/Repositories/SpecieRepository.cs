using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class SpecieRepository : GenericRepository<Specie>, ISpecieRepository
    {
        private readonly DataContext _context;

        public SpecieRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateBreedAsync(Models.Species.CreateBreedViewModel model)
        {
            var specie = await GetSpecieWithBreedsAsync(model.SpecieId);
            if (specie == null)
            {
                return;
            }

            specie.Breeds.Add(new Breed { Name = model.Name });
            _context.Species.Update(specie);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBreedAsync(Breed breed)
        {
            breed.IsDeleted = true;
            return await this.UpdateBreedAsync(breed);
        }

        public async Task<Breed> GetBreedByIdAsync(int id)
        {
            return await _context.Breeds.FindAsync(id);
        }

        public async Task<Specie> GetSpecieAsync(Breed breed)
        {
            return await _context.Species.Where(s => s.Breeds.Any(b => b.Id == breed.Id)).FirstOrDefaultAsync();
        }

        public IQueryable GetSpeciesWithBreeds()
        {
            return _context.Species
            .Include(s => s.Breeds)
            .OrderBy(s => s.Name);
        }

        public async Task<Specie> GetSpecieWithBreedsAsync(int id)
        {
            return await _context.Species
             .Include(s => s.Breeds)
             .Where(s => s.Id == id)
             .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateBreedAsync(Breed breed)
        {
            var specie = await _context.Species.Where(s => s.Breeds.Any(b => b.Id == breed.Id)).FirstOrDefaultAsync();
            if (specie == null)
            {
                return 0;
            }

            _context.Breeds.Update(breed);
            await _context.SaveChangesAsync();
            return specie.Id;
        }
    }
}
