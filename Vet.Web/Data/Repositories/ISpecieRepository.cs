using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface ISpecieRepository : IGenericRepository<Specie>
    {
        IQueryable GetSpeciesWithBreeds();

        Task<Specie> GetSpecieWithBreedsAsync(int id);

        Task<Breed> GetBreedByIdAsync(int id);

        Task CreateBreedAsync(Models.Species.CreateBreedViewModel model);

        Task<int> UpdateBreedAsync(Breed breed);

        Task<int> DeleteBreedAsync(Breed breed);

        Task<Specie> GetSpecieAsync(Breed breed);
    }
}