using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface IAnimalRepository : IGenericRepository<Animal>
    {
        IQueryable<Animal> GetClientAnimals(int clientId);
    }
}
