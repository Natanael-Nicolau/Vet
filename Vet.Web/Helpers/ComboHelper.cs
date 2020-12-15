using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;
using Vet.Web.Data.Repositories;

namespace Vet.Web.Helpers
{
    public class ComboHelper : IComboHelper
    {
        private readonly IUserRepository _userRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly ISpecieRepository _specieRepository;
        private readonly IInsuranceCompanyRepository _insuranceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private TimeSpan OpeningHours => new TimeSpan(8, 0, 0);
        private TimeSpan ClosingHours => new TimeSpan(20, 0, 0);

        public ComboHelper(
            IUserRepository userRepository ,
            ISpecialityRepository specialityRepository,
            ISpecieRepository specieRepository,
            IInsuranceCompanyRepository insuranceRepository,
            IDoctorRepository doctorRepository
            )
        {
            _userRepository = userRepository;
            _specialityRepository = specialityRepository;
            _specieRepository = specieRepository;
            _insuranceRepository = insuranceRepository;
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<SelectListItem> GetComboInsurances()
        {
            var list = _insuranceRepository.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an Insurance Company...)",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboRolesAsync()
        {
            var list = (await _userRepository.GetRolesAsync())
                .Where(r => r.Name != "Client")
                .OrderBy(r => r.Name);

            var comboList = new List<SelectListItem>();

            foreach (var role in list)
            {
                comboList.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            comboList.Insert(0, new SelectListItem
            {
                Text = "(Select a Role...)",
                Value = ""
            });


            return comboList;

        }

        public IEnumerable<SelectListItem> GetComboSpecialities()
        {
            var list = _specialityRepository.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Speciality...)",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboRoomsAsync(int specialityId)
        {
            var speciality = await _specialityRepository.GetSpecialityWithRoomsAsync(specialityId);


            var list = new List<SelectListItem>();
            if (speciality != null)
            {
                list = speciality.Rooms.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Room...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSpecies()
        {
            var list = _specieRepository.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a specie...)",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboBreedsAsync(int specieId)
        {
            var specie = await _specieRepository.GetSpecieWithBreedsAsync(specieId);


            var list = new List<SelectListItem>();
            if (specie != null)
            {
                list = specie.Breeds.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Breed...)",
                Value = "0"
            });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboDoctorsAsync(int roomId)
        {
            var list = await _doctorRepository.GetAllWithUserAsync()
                .Where(d => d.RoomId == roomId).Select(d => new SelectListItem
                {
                    Text = d.User.FullName,
                    Value = d.Id.ToString()

                }).OrderBy(l => l.Text).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecione um médico...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboAvailableAppointment(List<Appointment> reservedAppointments, TimeSpan appointmentTime)
        {
            List<TimeSpan> available = new List<TimeSpan>();
            List<Appointment> unavailable = reservedAppointments;
            TimeSpan duration = appointmentTime;


            if (appointmentTime.TotalMinutes != 0)
            {
                //Quando não há consultas marcadas
                if (unavailable.Count() == 0)
                {
                    AvailableAllDay(ref available, duration);
                }
                else
                {
                    AvailableStartDay(ref available, unavailable, duration);
                    AvailableBetweenAppointments(ref available, unavailable, duration);
                    AvailableEndDay(ref available, unavailable, duration);
                }
            }


            var list = new List<SelectListItem>();
            foreach (var i in available)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString("hh\\:mm"),
                    Value = i.TotalMinutes.ToString()
                });
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecione uma hora...)",
                Value = ""
            });

            return list;
        }

        #region Secret Sauce Pasta Code (Do not Open)
        /// <summary>
        /// Generates a complete list of the available schedules based on the duration of the doctor's appointments
        /// </summary>
        /// <param name="available"></param>
        /// /// <param name="duration"></param>
        private void AvailableAllDay(ref List<TimeSpan> available, TimeSpan duration)
        {
            available.Add(OpeningHours);
            while ((available.Last() + duration) <= (ClosingHours - duration))
            {
                available.Add((available.Last() + duration));
            }
        }

        /// <summary>
        /// Finds available schedules between the opening of the vet and the first locked appointment
        /// </summary>
        /// <param name="available">List where will be added the available schedules</param>
        /// <param name="unavailable">List of already scheduled appointments</param>
        /// <param name="duration">Duration of the doctor's appointments</param>
        private void AvailableStartDay(ref List<TimeSpan> available, List<Appointment> unavailable, TimeSpan duration)
        {
            if (unavailable.ElementAt(0).Start.Value.TimeOfDay > OpeningHours)
            {
                if ((unavailable.ElementAt(0).Start.Value.TimeOfDay - OpeningHours) >= duration)
                {
                    available.Add(OpeningHours);
                    while ((available.Last() + duration) <= (unavailable.ElementAt(0).Start.Value.TimeOfDay - duration))
                    {
                        available.Add((available.Last() + duration));
                    }
                }
            }
        }

        /// <summary>
        /// Finds available schedules inbetween already locked appointments
        /// </summary>
        /// <param name="available"> List where will be added the available schedules</param>
        /// <param name="unavailable">List of already scheduled appointments</param>
        /// <param name="duration">Duration of the doctor's appointments</param>
        private void AvailableBetweenAppointments(ref List<TimeSpan> available, List<Appointment> unavailable, TimeSpan duration)
        {
            for (int i = 0; i < unavailable.IndexOf(unavailable.Last()); i++)
            {
                if ((unavailable.ElementAt(i).End.Value.TimeOfDay) <= (unavailable.ElementAt(i + 1).Start.Value.TimeOfDay - duration))
                {
                    available.Add(unavailable.ElementAt(i).End.Value.TimeOfDay);
                    while ((available.Last() + duration) <= (unavailable.ElementAt(i + 1).Start.Value.TimeOfDay - duration))
                    {
                        available.Add((available.Last() + duration));
                    }
                }
            }
        }

        /// <summary>
        /// Finds available schedules between the last locked appointment and the closing of the vet
        /// </summary>
        /// <param name="available"> List where will be added the available schedules</param>
        /// <param name="unavailable">List of already scheduled appointments</param>
        /// <param name="duration">Duration of the doctor's appointments</param>
        private void AvailableEndDay(ref List<TimeSpan> available, List<Appointment> unavailable, TimeSpan duration)
        {
            if (unavailable.Last().End.Value.TimeOfDay <= ClosingHours - duration)
            {
                available.Add(unavailable.Last().End.Value.TimeOfDay);
                while ((available.Last() + duration) <= (ClosingHours - duration))
                {
                    available.Add((available.Last() + duration));
                }
            }
        }
        #endregion

    }
}
