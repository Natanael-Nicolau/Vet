using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext Context) : base(Context)
        {
            _context = Context;
        }

        public async Task<Client> GetClientByUserId(string userId)
        {
            return await _context.Clients
                .Include(c => c.User)
                .Where(c => c.User.Id == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Client> GetClientByUserEmail(string email)
        {
            return await _context.Clients
                .Include(c => c.User)
                .Where(c => c.User.Id == email)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
