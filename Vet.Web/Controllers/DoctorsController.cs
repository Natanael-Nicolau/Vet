using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;

namespace Vet.Web.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IConverterHelper _converterHelper;

        public DoctorsController(
            IUserRepository userRepository,
            IDoctorRepository doctorRepository,
            ISpecialityRepository specialityRepository,
            IConverterHelper converterHelper)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _converterHelper = converterHelper;
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Details()
        {
            var doctor = await _doctorRepository.GetDoctorByUserEmail(this.User.Identity.Name);
            if (doctor == null || doctor.IsDeleted)
            {
                return NotFound();
            }

            doctor.User = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
            if (doctor.User == null || doctor.User.IsDeleted)
            {
                return NotFound();
            }

            doctor.Room = await _specialityRepository.GetRoomByIdAsync(doctor.RoomId);
            if (doctor.Room == null)
            {
                return NotFound();
            }

            doctor.Room.Speciality = await _specialityRepository.GetSpecialityAsync(doctor.Room);
            if (doctor.Room.Speciality == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToDoctorDetailsViewModel(doctor);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetailsForAdmin(string userId)
        {
            var doctor = await _doctorRepository.GetDoctorByUserId(userId);
            if (doctor == null || doctor.IsDeleted)
            {
                return NotFound();
            }

            doctor.User = await _userRepository.GetUserByIdAsync(userId);
            if (doctor.User == null || doctor.User.IsDeleted)
            {
                return NotFound();
            }

            doctor.Room = await _specialityRepository.GetRoomByIdAsync(doctor.RoomId);
            if (doctor.Room == null)
            {
                return NotFound();
            }

            doctor.Room.Speciality = await _specialityRepository.GetSpecialityAsync(doctor.Room);
            if (doctor.Room.Speciality == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToDoctorDetailsViewModel(doctor);
            return View(nameof(Details), model);
        }
    }
}
