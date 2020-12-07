using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client> GetClientByUserId(string userId);

        Task<Client> GetClientByUserEmail(string email);
    }
}
