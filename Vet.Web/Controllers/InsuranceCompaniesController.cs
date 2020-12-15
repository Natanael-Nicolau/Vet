using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.InsuranceCompanies;

namespace Vet.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class InsuranceCompaniesController : Controller
    {
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IConverterHelper _converterHelper;

        public InsuranceCompaniesController(
            IInsuranceCompanyRepository insuranceCompanyRepository,
            IConverterHelper converterHelper)
        {
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _converterHelper = converterHelper;
        }


        public IActionResult Index()
        {
            var model = _insuranceCompanyRepository.GetAll()
                .Select(i => new IndexViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    DiscountPercent = i.DiscountPercent
                }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var insurance = await _insuranceCompanyRepository.GetByIdAsync(id.Value);
            if (insurance == null)
            {
                return this.NotFound();
            }

            var model = _converterHelper.ToDetailsViewModel(insurance);
            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var insurance = _converterHelper.ToInsuranceCompanyEntity(model);

                await _insuranceCompanyRepository.CreateAsync(insurance);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var insurance = await _insuranceCompanyRepository.GetByIdAsync(id.Value);
            if (insurance == null)
            {
                return this.NotFound();
            }

            var model = _converterHelper.ToEditViewModel(insurance);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var insurance = _converterHelper.ToInsuranceCompanyEntity(model);

                await _insuranceCompanyRepository.UpdateAsync(insurance);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(404, "Insurance Company not Found");
            }

            var insurance = await _insuranceCompanyRepository.GetByIdAsync(id.Value);
            if (insurance == null)
            {
                return StatusCode(404, "Insurance Company not Found");
            }
            else
            {
                await _insuranceCompanyRepository.DeleteAsync(insurance);
            }
            return StatusCode(200, "Insurance Company Successfully Deleted");
        }

    }
}
