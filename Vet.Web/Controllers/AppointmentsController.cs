using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Entities;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.Appointments;

namespace Vet.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IComboHelper _comboHelper;
        private readonly IConverterHelper _converterHelper;


        public AppointmentsController(
            IAppointmentRepository appointmentRepository,
            IAnimalRepository animalRepository,
            IDoctorRepository doctorRepository,
            IClientRepository clientRepository,
            ISpecialityRepository specialityRepository,
            IUserRepository userRepository,
            IComboHelper comboHelper,
            IConverterHelper converterHelper)
        {
            _appointmentRepository = appointmentRepository;
            _animalRepository = animalRepository;
            _doctorRepository = doctorRepository;
            _clientRepository = clientRepository;
            _specialityRepository = specialityRepository;
            _userRepository = userRepository;
            _comboHelper = comboHelper;
            _converterHelper = converterHelper;
        }


        
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {

            var model = (await _appointmentRepository.GetAllAdmin())
                .Select(a => new IndexViewModel {
                Id = a.Id,
                Start = a.Start,
                AnimalName = a.Animal.Name,
                DoctorName = a.Doctor.User.FullName,
                End = a.End,
                IsAproved = a.IsAproved
                }).OrderByDescending(a => a.Start).ToList();         

            return View(model);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientIndex()
        {
            var client = await _clientRepository.GetClientByUserEmail(this.User.Identity.Name);
            if (client == null)
            {
                return NotFound();
            }

            var model = (await _appointmentRepository.GetAllClient(client.Id))
                .Select(a => new IndexViewModel
                {
                    Id = a.Id,
                    Start = a.Start,
                    AnimalName = a.Animal.Name,
                    DoctorName = a.Doctor.User.FullName,
                    End = a.End,
                    IsAproved = a.IsAproved
                }).OrderByDescending(a => a.Start).ToList();

            return View(model);
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorIndex(bool isApproval)
        {
            var doctor = await _doctorRepository.GetDoctorByUserEmail(this.User.Identity.Name);
            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorIndexViewModel
            {
                IsApproval = isApproval
            };
            if (model.IsApproval)
            {
                 model.ModelList = (await _appointmentRepository.GetNotAprovedDoctor(doctor.Id))
                .Select(a => new IndexViewModel
                {
                    Id = a.Id,
                    Start = a.Start,
                    AnimalName = a.Animal.Name,
                    DoctorName = a.Doctor.User.FullName,
                    End = a.End,
                    IsAproved = a.IsAproved
                }).OrderByDescending(a => a.Start).ToList();

            }
            else
            {

                model.ModelList = (await _appointmentRepository.GetAllDoctor(doctor.Id))
                .Select(a => new IndexViewModel
                {
                    Id = a.Id,
                    Start = a.Start,
                    AnimalName = a.Animal.Name,
                    DoctorName = a.Doctor.User.FullName,
                    End = a.End,
                    IsAproved = a.IsAproved
                }).OrderByDescending(a => a.Start).ToList();
            }

            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentRepository.GetAppointmentWithAllAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToDetailsViewModel(appointment);
            return View(model);
        }


        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }
            var animal = await _animalRepository.GetByIdAsync(id.Value);
            if (animal == null)
            {
                return this.NotFound();
            }

            var model = new CreateViewModel
            {
                AnimalId = animal.Id,
                AnimalName = animal.Name,
                Day = DateTime.Now,
                Specialities = _comboHelper.GetComboSpecialities(),
                Rooms = await _comboHelper.GetComboRoomsAsync(0),
                Doctors = await _comboHelper.GetComboDoctorsAsync(0),
                AvailableHours = _comboHelper.GetComboAvailableAppointment(new List<Appointment>(), new TimeSpan())
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Appointment
                {
                    Start = model.Day + new TimeSpan(0, model.AppointmentTime, 0),
                    AnimalId = model.AnimalId,
                    DoctorId = model.DoctorId
                };

                await _appointmentRepository.CreateAsync(entity);

                ViewBag.Message = "Appointment Successfully Requested";
                return View(model);
            }

            model.Specialities = _comboHelper.GetComboSpecialities();
            model.Rooms = await _comboHelper.GetComboRoomsAsync(model.SpecialityId);
            model.Doctors = await _comboHelper.GetComboDoctorsAsync(model.RoomId);
            model.AvailableHours = _comboHelper.GetComboAvailableAppointment(new List<Appointment>(), new TimeSpan());
            return View(model);
        }

        
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Appointment Not Found!");
            }
            var appointment = await _appointmentRepository.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return StatusCode(404, "Appointment Not Found!");
            }
            await _appointmentRepository.DeleteAsync(appointment);
            return StatusCode(200);
        }

        [Authorize(Roles ="Doctor")]
        public async Task<IActionResult> Aprove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appointment = await _appointmentRepository.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.IsAproved = true;
            await _appointmentRepository.UpdateAsync(appointment);
            return RedirectToAction(nameof(DoctorIndex), new { isApproval = true });
        }





        [Authorize(Roles = "Client,Admin")]
        public async Task<JsonResult> GetRoomsAsync(int specialityId)
        {
            var rooms = await _comboHelper.GetComboRoomsAsync(specialityId);
            return this.Json(rooms);
        }

        [Authorize(Roles = "Client,Admin")]
        public async Task<JsonResult> GetDoctorsAsync(int roomId)
        {
            var doctors = await _comboHelper.GetComboDoctorsAsync(roomId);
            return this.Json(doctors);
        }


        [Authorize(Roles = "Client,Admin")]
        public async Task<JsonResult> GetAvailableAppointmentsAsync(int? doctorId, DateTime day)
        {
            if (doctorId == null)
            {
                throw new Exception("error getting unavailable appointments");
            }
            var doctor = await _doctorRepository.GetByIdAsync(doctorId.Value);
            if (doctor == null)
            {
                throw new Exception("error getting unavailable appointments");
            }

            var appointments = _appointmentRepository.GetUnavailableAppointments(day, doctor.RoomId).ToList();
            if (appointments == null)
            {
                throw new Exception("error getting unavailable appointments");
            }
            var availableHours = _comboHelper.GetComboAvailableAppointment(appointments, (doctor == null ? new TimeSpan() : doctor.AppointmentDuration));
            return this.Json(availableHours);
        }
    }
}
