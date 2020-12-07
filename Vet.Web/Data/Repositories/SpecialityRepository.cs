using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public class SpecialityRepository : GenericRepository<Speciality>, ISpecialityRepository
    {
        private readonly DataContext _context;

        public SpecialityRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateRoomAsync(Models.Specialities.CreateRoomViewModel model)
        {
            var speciality = await GetSpecialityWithRoomsAsync(model.SpecialityId);
            if (speciality == null)
            {
                return;
            }

            speciality.Rooms.Add(new Room { Name = model.Name });
            _context.Specialities.Update(speciality);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRoomAsync(Room room)
        {
            room.IsDeleted = true;
            _context.Rooms.Remove(room);
            return await this.UpdateRoomAsync(room);
        }


        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public IQueryable GetSpecialitiesWithRooms()
        {
            return _context.Specialities
            .Include(s => s.Rooms)
            .OrderBy(s => s.Name);
        }

        public async Task<Speciality> GetSpecialityAsync(Room room)
        {
            return await _context.Specialities.Where(s => s.Rooms.Any(r => r.Id == room.Id)).FirstOrDefaultAsync();
        }

        public async Task<Speciality> GetSpecialityWithRoomsAsync(int id)
        {
            return await _context.Specialities
             .Include(s => s.Rooms)
             .Where(s => s.Id == id)
             .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateRoomAsync(Room room)
        {
            var speciality = await _context.Specialities.Where(s => s.Rooms.Any(r => r.Id == room.Id)).FirstOrDefaultAsync();
            if (speciality == null)
            {
                return 0;
            }

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return speciality.Id;
        }
    }
}
