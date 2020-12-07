using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.Account;

namespace Vet.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(
            IInsuranceCompanyRepository insuranceCompanyRepository,
            IClientRepository clientRepository,
            IEmployeeRepository employeeRepository,
            IDoctorRepository doctorRepository,
            ISpecialityRepository specialityRepository,
            IUserRepository userRepository,
            IConfiguration configuration,
            IMailHelper mailHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper

            )
        {
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        #region Login/out

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                var role = await _userRepository.GetUserRoleAsync(user);

                return RedirectToAction("Index", role);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    var user = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    var role = await _userRepository.GetUserRoleAsync(user);
                    return RedirectToAction("Index", role);
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);

        }
        public async Task<IActionResult> Logout()
        {
            await _userRepository.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
