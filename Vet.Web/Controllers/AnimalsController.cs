using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.Animals;

namespace Vet.Web.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly ISpecieRepository _specieRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IComboHelper _comboHelper;
        private readonly IImageHelper _imageHelper;

        public AnimalsController(
            IAnimalRepository animalRepository,
            ISpecieRepository specieRepository,
            IClientRepository clientRepository,
            IUserRepository userRepository,
            IConverterHelper converterHelper,
            IComboHelper comboHelper,
            IImageHelper imageHelper)
        {
            _animalRepository = animalRepository;
            _specieRepository = specieRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _converterHelper = converterHelper;
            _comboHelper = comboHelper;
            _imageHelper = imageHelper;
        }

        [Authorize(Roles ="Client")]
        public async Task<IActionResult> ClientIndex()
        {
            var client = await _clientRepository.GetClientByUserEmail(this.User.Identity.Name);
            if (client == null)
            {
                return NotFound();
            }
            else
            {
                return RedirectToAction(nameof(Index), new { id = client.Id });
            }
        }


        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }           


            client.User = await _userRepository.GetUserByIdAsync(client.UserId);
            if (client.User == null)
            {
                return this.NotFound();
            }
            if (this.User.IsInRole("Client"))
            {
                if (this.User.Identity.Name != client.User.Email)
                {
                    return this.Unauthorized();
                }
            }

            client.Animals = _animalRepository.GetClientAnimals(client.Id).ToList();

            var model = _converterHelper.ToIndexViewModel(client);
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Details(int? id)
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
            animal.Breed = await _specieRepository.GetBreedByIdAsync(animal.BreedId);
            if (animal.Breed == null)
            {
                return this.NotFound();
            }
            animal.Breed.Specie = await _specieRepository.GetSpecieAsync(animal.Breed);
            if (animal.Breed.Specie == null)
            {
                return this.NotFound();
            }

            var model = _converterHelper.ToDetailsViewModel(animal);


            return View(model);
        }

        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }



            var model = new CreateViewModel
            {
                OwnerId = id.Value,
                Species = _comboHelper.GetComboSpecies(),
                Breeds = await _comboHelper.GetComboBreedsAsync(0)
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
                var path = string.Empty;
                if (model.Photo != null && model.Photo.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.Photo, "Animals");
                }

                var animal = _converterHelper.ToAnimalEntity(model, path);
                await _animalRepository.CreateAsync(animal);

                return RedirectToAction(nameof(Index), new { id = model.OwnerId });
            }

            model.Species = _comboHelper.GetComboSpecies();
            model.Breeds = await _comboHelper.GetComboBreedsAsync(model.SpecieId);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
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

            animal.Breed = await _specieRepository.GetBreedByIdAsync(animal.BreedId);
            if (animal.Breed == null)
            {
                return this.NotFound();
            }
            animal.Breed.Specie = await _specieRepository.GetSpecieAsync(animal.Breed);
            if (animal.Breed.Specie == null)
            {
                return this.NotFound();
            }

            var model = _converterHelper.ToEditViewModel(animal);

            model.Species = _comboHelper.GetComboSpecies();
            model.Breeds = await _comboHelper.GetComboBreedsAsync(model.SpecieId);
            return View(model);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.PictureUrl;
                if (model.Photo != null && model.Photo.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.Photo, "Animals");
                }

                var animal = _converterHelper.ToAnimalEntity(model, path);
                await _animalRepository.UpdateAsync(animal);


                return RedirectToAction(nameof(Details), new { id = animal.Id });
            }

            model.Species = _comboHelper.GetComboSpecies();
            model.Breeds = await _comboHelper.GetComboBreedsAsync(model.SpecieId);
            return View(model);
        }


        [Authorize(Roles ="Client,Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Animal not Found");
            }


            var animal = await _animalRepository.GetByIdAsync(id.Value);
            if (animal == null)
            {
                return StatusCode(404, "Animal not Found");
            }

            await _animalRepository.DeleteAsync(animal);
            return StatusCode(200, "Animal successfully deleted");
        }



        public async Task<JsonResult> GetBreedsAsync(int specieId)
        {
            var breeds = await _comboHelper.GetComboBreedsAsync(specieId);
            return this.Json(breeds);
        }
    }
}
