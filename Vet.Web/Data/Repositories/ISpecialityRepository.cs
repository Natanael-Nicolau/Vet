using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface ISpecialityRepository : IGenericRepository<Speciality>
    {
        IQueryable GetSpecialitiesWithRooms();


        Task<Speciality> GetSpecialityWithRoomsAsync(int id);


        Task<Room> GetRoomByIdAsync(int id);


        Task CreateRoomAsync(Models.Specialities.CreateRoomViewModel model);


        Task<int> UpdateRoomAsync(Room room);


        Task<int> DeleteRoomAsync(Room room);

        Task<Speciality> GetSpecialityAsync(Room room);

    }
}
