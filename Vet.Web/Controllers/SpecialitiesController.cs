using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.Specialities;

namespace Vet.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SpecialitiesController : Controller
    {
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IConverterHelper _converterHelper;

        public SpecialitiesController(
            ISpecialityRepository specialityRepository,
            IConverterHelper converterHelper)
        {
            _specialityRepository = specialityRepository;
            _converterHelper = converterHelper;
        }


        #region SpecialitiesCRUD
        // GET: SpecialitiesController
        public IActionResult Index()
        {
            var model = _specialityRepository.GetSpecialitiesWithRooms()
                .Select(s => new IndexViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfRooms = s.NumberOfRooms
                });

            return View(model);
        }

        // GET: SpecialitiesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _specialityRepository.GetSpecialityWithRoomsAsync(id.Value);
            if (speciality == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToDetailsViewModel(speciality);
            return View(model);
        }

        // GET: SpecialitiesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpecialitiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var speciality = _converterHelper.ToSpecialityEntity(model);
                await _specialityRepository.CreateAsync(speciality);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: SpeciesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _specialityRepository.GetByIdAsync(id.Value);
            if (speciality == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToEditViewModel(speciality);
            return View(model);
        }

        // POST: SpecialitiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var speciality = _converterHelper.ToSpecialityEntity(model);
                await _specialityRepository.UpdateAsync(speciality);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: SpecialitiesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Speciality not Found");
            }

            var speciality = await _specialityRepository.GetSpecialityWithRoomsAsync(id.Value);
            if (speciality == null)
            {
                return StatusCode(404, "Speciality not Found");
            }


            await _specialityRepository.DeleteAsync(speciality);
            return StatusCode(200, "Speciality Successfully deleted");
        }
        #endregion

        #region RoomCRUD

        public async Task<IActionResult> CreateRoom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speciality = await _specialityRepository.GetByIdAsync(id.Value);
            if (speciality == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCreateRoomViewModel(speciality);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom(CreateRoomViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _specialityRepository.CreateRoomAsync(model);
                return this.RedirectToAction($"Details/{model.SpecialityId}");
            }

            return this.View(model);
        }

        public async Task<IActionResult> EditRoom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _specialityRepository.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToEditRoomViewModel(room);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(EditRoomViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var room = _converterHelper.ToRoomEntity(model);
                var specialityId = await _specialityRepository.UpdateRoomAsync(room);
                if (specialityId != 0)
                {
                    return this.RedirectToAction($"Details/{specialityId}");
                }
            }

            return View(model);
        }

        
        public async Task<IActionResult> DeleteRoom(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Room not Found");
            }

            var room = await _specialityRepository.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return StatusCode(404, "Room not Found");
            }

            var specialityId = await _specialityRepository.DeleteRoomAsync(room);
            return StatusCode(200, "Room Successfully Deleted");
        }
        #endregion
    }
}
