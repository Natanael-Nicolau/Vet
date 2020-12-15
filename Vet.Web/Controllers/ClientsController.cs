using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Entities;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;

namespace Vet.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IConverterHelper _converterHelper;

        public ClientsController(
            IAnimalRepository animalRepository,
            IClientRepository clientRepository,
            IUserRepository userRepository,
            IInsuranceCompanyRepository insuranceCompanyRepository,
            IConverterHelper converterHelper)
        {
            _animalRepository = animalRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _converterHelper = converterHelper;
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Details()
        {
            var client = await _clientRepository.GetClientByUserEmail(this.User.Identity.Name);
            if (client == null || client.IsDeleted)
            {
                return NotFound();
            }

            client.User = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
            if (client.User == null || client.User.IsDeleted)
            {
                return NotFound();
            }

            if (client.InsuranceCompanyId != null)
            {
                client.InsuranceCompany = await _insuranceCompanyRepository.GetByIdAsync(client.InsuranceCompanyId.Value);
            }
            else
            {
                client.InsuranceCompany = new InsuranceCompany();
            }
            client.Animals = _animalRepository.GetClientAnimals(client.Id).ToList();
            var model = _converterHelper.ToClientDetailsViewModel(client);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetailsForAdmin(string userId)
        {
            var client = await _clientRepository.GetClientByUserId(userId);
            if (client == null || client.IsDeleted)
            {
                return NotFound();
            }

            client.User = await _userRepository.GetUserByIdAsync(userId);
            if (client.User == null || client.User.IsDeleted)
            {
                return NotFound();
            }

            if (client.InsuranceCompanyId != null)
            {
                client.InsuranceCompany = await _insuranceCompanyRepository.GetByIdAsync(client.InsuranceCompanyId.Value);
            }
            else
            {
                client.InsuranceCompany = new InsuranceCompany();
            }
            client.Animals = _animalRepository.GetClientAnimals(client.Id).ToList();
            var model = _converterHelper.ToClientDetailsViewModel(client);
            return View(nameof(Details), model);
        }
    }
}
