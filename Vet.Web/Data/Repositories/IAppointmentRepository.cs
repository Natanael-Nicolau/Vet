using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        IQueryable<Appointment> GetMonthAppointmentsForClient(int clientId);
        IQueryable<Appointment> GetMonthAppointmentsForDoctor(int doctorId);
        IQueryable<Appointment> GetMonthAppointmentsForAll();


        IQueryable<Appointment> GetDayAppointmentsForClient(int clientId);
        IQueryable<Appointment> GetDoctorAppointmentsDay(int doctorId);
        IQueryable<Appointment> GetAllAppointmentsDay();

        IQueryable<Appointment> GetUnavailableAppointments(DateTime day, int roomId);

        IQueryable<Appointment> GetAllClient(int clientId);
    }
}
