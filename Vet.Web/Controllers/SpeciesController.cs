using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.Species;

namespace Vet.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SpeciesController : Controller
    {
        private readonly ISpecieRepository _specieRepository;
        private readonly IConverterHelper _converterHelper;

        public SpeciesController(
            ISpecieRepository specieRepository,
            IConverterHelper converterHelper)
        {
            _specieRepository = specieRepository;
            _converterHelper = converterHelper;
        }


        #region SpeciesCRUD
        // GET: SpeciesController
        public IActionResult Index()
        {
            var model = _specieRepository.GetSpeciesWithBreeds()
                .Select(s => new IndexViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfBreeds = s.NumberOfBreeds
                });

            return View(model);
        }

        // GET: SpeciesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _specieRepository.GetSpecieWithBreedsAsync(id.Value);
            if (specie == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToDetailsViewModel(specie);
            return View(model);
        }

        // GET: SpeciesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpeciesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var specie = _converterHelper.ToSpecieEntity(model);
                await _specieRepository.CreateAsync(specie);
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

            var specie = await _specieRepository.GetByIdAsync(id.Value);
            if (specie == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToEditViewModel(specie);
            return View(model);
        }

        // POST: SpeciesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var specie = _converterHelper.ToSpecieEntity(model);
                await _specieRepository.UpdateAsync(specie);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: SpeciesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Species not Found");
            }

            var specie = await _specieRepository.GetSpecieWithBreedsAsync(id.Value);
            if (specie == null)
            {
                return StatusCode(404, "Species not Found");
            }


            await _specieRepository.DeleteAsync(specie);
            return StatusCode(200, "Species successfully deleted");
        }
        #endregion

        #region BreedCRUD

        public async Task<IActionResult> CreateBreed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _specieRepository.GetByIdAsync(id.Value);
            if (specie == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCreateBreedViewModel(specie);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBreed(CreateBreedViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _specieRepository.CreateBreedAsync(model);
                return this.RedirectToAction($"Details/{model.SpecieId}");
            }

            return this.View(model);
        }

        public async Task<IActionResult> EditBreed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var breed = await _specieRepository.GetBreedByIdAsync(id.Value);
            if (breed == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToEditBreedViewModel(breed);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBreed(EditBreedViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var breed = _converterHelper.ToBreedEntity(model);
                var specieId = await _specieRepository.UpdateBreedAsync(breed);
                if (specieId != 0)
                {
                    return this.RedirectToAction($"Details/{specieId}");
                }
            }

            return View(model);
        }


        public async Task<IActionResult> DeleteBreed(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Breed not Found");
            }

            var breed = await _specieRepository.GetBreedByIdAsync(id.Value);
            if (breed == null)
            {
                return StatusCode(404, "Breed not Found");
            }

            var specialityId = await _specieRepository.DeleteBreedAsync(breed);
            return StatusCode(200, "Breed Successfully Deleted");
        }
        #endregion
    }
}
