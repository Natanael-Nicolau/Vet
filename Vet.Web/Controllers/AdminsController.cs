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
    public class AdminsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConverterHelper _converterHelper;

        public AdminsController(
            IUserRepository userRepository,
            IConverterHelper converterHelper)
        {
            _userRepository = userRepository;
            _converterHelper = converterHelper;
        }


        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Details()
        {
            var user = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }

            var model = _converterHelper.ToAdminDetailsViewModel(user);
            return View(model);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DetailsForAdmin(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }

            var model = _converterHelper.ToAdminDetailsViewModel(user);
            return View(nameof(Details), model);
        }




    }
}
