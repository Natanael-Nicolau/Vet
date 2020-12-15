using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vet.Web.Data.Repositories;
using Vet.Web.Models;

namespace Vet.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public HomeController(
            IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DoctorsIndex()
        {
            var model = _doctorRepository.GetAllWithUsersAndSpecialityAsync()
                .Select(d => new DoctorsIndexViewModel
                {
                    PhotoUrl = d.User.PictureUrl,
                    DoctorSpeciality = d.Room.Speciality.Name,
                    DoctorFullName = d.User.FullName
                }).ToList();

            return View(model);
                
        }


        [Authorize]
        public IActionResult LoggedIndex()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
