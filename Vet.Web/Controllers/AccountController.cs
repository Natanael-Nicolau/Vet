using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Vet.Web.Data.Repositories;
using Vet.Web.Helpers;
using Vet.Web.Models.Account;

namespace Vet.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IInsuranceCompanyRepository _insuranceCompanyRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecialityRepository _specialityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IComboHelper _comboHelper;

        public AccountController(
            IAppointmentRepository appointmentRepository,
            IInsuranceCompanyRepository insuranceCompanyRepository,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository,
            ISpecialityRepository specialityRepository,
            IUserRepository userRepository,
            IConfiguration configuration,
            IMailHelper mailHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper,
            IComboHelper comboHelper
            )
        {
            _appointmentRepository = appointmentRepository;
            _insuranceCompanyRepository = insuranceCompanyRepository;
            _clientRepository = clientRepository;
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _comboHelper = comboHelper;
        }


        #region CRUD


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateViewModel
            {
                Roles = await _comboHelper.GetComboRolesAsync(),
                Specialities = _comboHelper.GetComboSpecialities(),
                Rooms = await _comboHelper.GetComboRoomsAsync(0),
                DateOfBirth = DateTime.Now.Date
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (model.Role == "Doctor")
            {
                if (model.AppointmentDuration <= 0 || model.AppointmentDuration >= 720) //720 minutes == 12 hours which is max time the vet is open
                {
                    ModelState.AddModelError(nameof(model.AppointmentDuration), "Please select a value between 1 and 719.");
                }
                if (model.SpecialityId < 1)
                {
                    ModelState.AddModelError(nameof(model.SpecialityId), "Please select a Speciality.");
                }
                if (model.RoomId < 1)
                {
                    ModelState.AddModelError(nameof(model.RoomId), "Please select a Room.");
                }
            }
            if (ModelState.IsValid)
            {
                //Gets the user
                var user = await _userRepository.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    // Initializes an empty path
                    string path = string.Empty;
                    if (model.Photo != null && model.Photo.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.Photo, "Users");
                    }
                    else
                    {
                        //Defines the Photo as the default
                        path = $"~/images/Users/Default_User_Image.png";
                    }

                    //Initializes a new User
                    user = _converterHelper.ToUserEntity(model, path);

                    // Adds the User to the DataBase
                    var result = await _userRepository.AddUserAsync(user, "Password");
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The User could not be created");
                        return View(model);
                    }

                    // Add user to Role
                    await _userRepository.AddUserToRoleAsync(user, model.Role);

                    if (model.Role == "Doctor")
                    {
                        var doctor = _converterHelper.ToDoctorEntity(model, user.Id);

                        await _doctorRepository.CreateAsync(doctor);
                    }

                    // Creates a Token in order to confirm the email
                    var myToken = await _userRepository.GenerateEmailConfirmationTokenAsync(user);

                    // Defines the Link with its properties to be sent in the email
                    var tokenLink = this.Url.Action("ConfirmAccount", "Account", new
                    {
                        userId = user.Id,
                        token = myToken,
                    }, protocol: HttpContext.Request.Scheme);

                    //Sends an Email to the User with the TokenLink
                    try
                    {
                        _mailHelper.SendMail(model.Email, "Account Confirmation", $"<h1>Account Confirmation</h1>" +
                       $"To finish your account registration, " +
                       $"please click this link:</br></br><a href = \"{tokenLink}\">Confirm Account</a>");
                        this.ViewBag.Message = "To finish account creation verify your email.";
                    }
                    catch (Exception)
                    {
                        this.ModelState.AddModelError(string.Empty, "Error sending the email, please try again in a few minutes");
                    }

                    model.Roles = await _comboHelper.GetComboRolesAsync();
                    model.Specialities = _comboHelper.GetComboSpecialities();
                    model.Rooms = await _comboHelper.GetComboRoomsAsync(model.SpecialityId);
                    return this.View(model);
                }

                ModelState.AddModelError(nameof(model.Email), "A User with this email is already registered");
            }


            model.Roles = await _comboHelper.GetComboRolesAsync();
            model.Specialities = _comboHelper.GetComboSpecialities();
            model.Rooms = await _comboHelper.GetComboRoomsAsync(model.SpecialityId);
            return View(model);
        }

        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                InsuranceCompanies = _comboHelper.GetComboInsurances(),
                DateOfBirth = DateTime.Now.Date
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Gets the user
                var user = await _userRepository.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    // Defines the photo as the default
                    string path = $"~/images/Users/Default_User_Image.png";

                    //Initializes a new User
                    user = _converterHelper.ToUserEntity(model, path);

                    // Adds the User to the DataBase
                    var result = await _userRepository.AddUserAsync(user, "Password");
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The User could not be created");
                        return View(model);
                    }

                    // Add user to Role
                    await _userRepository.AddUserToRoleAsync(user, "Client");


                    //Initializes a new Client
                    var client = _converterHelper.ToClientEntity(model, user.Id);
                    await _clientRepository.CreateAsync(client);


                    // Creates a Token in order to confirm the email
                    var myToken = await _userRepository.GenerateEmailConfirmationTokenAsync(user);

                    // Defines the Link with its properties to be sent in the email
                    var tokenLink = this.Url.Action("ConfirmAccount", "Account", new
                    {
                        userId = user.Id,
                        token = myToken,
                    }, protocol: HttpContext.Request.Scheme);

                    //Sends an Email to the User with the TokenLink
                    try
                    {
                        _mailHelper.SendMail(model.Email, "Account Confirmation", $"<h1>Account Confirmation</h1>" +
                       $"To confirme your email, " +
                       $"please click this link:</br></br><a href = \"{tokenLink}\">Confirm Account</a>");
                        this.ViewBag.Message = "Please check your email to finish your registration.";
                    }
                    catch (Exception)
                    {
                        this.ModelState.AddModelError(string.Empty, "Error sending the email, please try again in a few minutes");
                    }

                    model.InsuranceCompanies = _comboHelper.GetComboInsurances();
                    return this.View(model);
                }
            }

            model.InsuranceCompanies = _comboHelper.GetComboInsurances();
            return View(model);
        }




        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var userList = await _userRepository.GetAllUsersAsync();

            ICollection<IndexViewModel> modelList = new List<IndexViewModel>();
            foreach (var user in userList)
            {
                var role = await _userRepository.GetUserRoleAsync(user);
                var model = _converterHelper.ToIndexViewModel(user, role);

                modelList.Add(model);
            }

            return View(modelList.OrderBy(m => m.FullName));
        }


        [Authorize]
        public async Task<IActionResult> Details()
        {
            var user = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            var role = await _userRepository.GetUserRoleAsync(user);

            switch (role)
            {
                case "Admin":
                    return RedirectToAction("Details", "Admins");
                case "Doctor":
                    return RedirectToAction("Details", "Doctors");
                case "Client":
                    return RedirectToAction("Details", "Clients");
                default:
                    return NotFound();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetailsForAdmin(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var role = await _userRepository.GetUserRoleAsync(user);

            switch (role)
            {
                case "Admin":
                    return RedirectToAction("DetailsForAdmin", "Admins", new { userId = user.Id });
                case "Doctor":
                    return RedirectToAction("DetailsForAdmin", "Doctors", new { userId = user.Id });
                case "Client":
                    return RedirectToAction("DetailsForAdmin", "Clients", new { userId = user.Id });
                default:
                    return NotFound();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return StatusCode(404, "User Not Found");
            }

            var role = await _userRepository.GetUserRoleAsync(user);
            if (role == null)
            {
                return StatusCode(404, "User Not Found");
            }

            switch (role)
            {
                case "Admin":
                    await _userRepository.DeleteUserAsync(user);
                    break;
                case "Doctor":
                    var doctor = await _doctorRepository.GetDoctorByUserEmail(user.Email);
                    if (doctor == null)
                    {
                        return StatusCode(404, "User Not Found");
                    }

                    //Checks if the doctor still has at least 1 appointment
                    var appointment = _appointmentRepository.GetAll().FirstOrDefault(a => a.DoctorId == doctor.Id);
                    if (appointment != null)
                    {
                        return StatusCode(400, "The Doctor you're trying to delete still has pending appointments");
                    }

                    await _doctorRepository.DeleteAsync(doctor);
                    await _userRepository.DeleteUserAsync(user);
                    break;

                case "Client":
                    var client = await _clientRepository.GetClientByUserEmail(user.Email);
                    if (client == null)
                    {
                        return StatusCode(404, "User Not Found");
                    }
                    //Checks if the client still has at least 1 animal
                    if (client.NumberOfAnimals > 0)
                    {
                        return StatusCode(400, "The Client you're trying to delete still has registered animals");
                    }

                    await _clientRepository.DeleteAsync(client);
                    await _userRepository.DeleteUserAsync(user);
                    break;

                default:
                    return StatusCode(404, "User Not Found");
            }

            return StatusCode(200, "Success");

        }

        #endregion


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

                return RedirectToAction("LoggedIndex", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(model.Username);
                if ((user == null || user.IsDeleted))
                {
                    var result = await _userRepository.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(Request.Query["ReturnUrl"].First());
                        }


                        return RedirectToAction("LoggedIndex", "Home");
                    }
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


        #region Password Managment
        public async Task<IActionResult> ConfirmAccount(string userId, string token)
        {
            // Verifies if the userId and token are not empty
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return this.NotFound();
            }

            // Gets the User through its Id
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            // Initializes a new Model, filling the fields 
            var model = new ResetPasswordViewModel
            {
                Username = user.UserName,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAccount(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    // Confirms the Email
                    var result = await _userRepository.ConfirmEmailAsync(user, model.Token);
                    if (!result.Succeeded)
                    {
                        return this.NotFound();
                    }

                    // Changes the default password to the user chosen password"
                    result = await _userRepository.ChangePasswordAsync(user, "Password", model.Password);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("Login");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "Error while changing the password.");
                        return View(model);
                    }


                }

                this.ModelState.AddModelError(string.Empty, "User not found.");
                return View(model);
            }

            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "O email não tem um utilizador associado.");
                    return this.View(model);
                }

                var myToken = await _userRepository.GeneratePasswordResetTokenAsync(user);

                var link = Url.Action("ResetPassword", "Account",
                    new {
                        userId = user.Id,
                        token = myToken
                    },
                    protocol: HttpContext.Request.Scheme,
                    HttpContext.Request.Host.Value);


                _mailHelper.SendMail(model.Email, "Reset Password E-Vet",
                $"<h1>E-Vet Password Reset</h1>" +
                $"To reset your password click the link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to reset your password have been sent to your email.";

            }

            return View(model);
        }

        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            // Verifies if the userId and token are not empty
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return this.NotFound();
            }

            // Gets the User through its Id
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            // Initializes a new Model, filling the fields 
            var model = new ResetPasswordViewModel
            {
                Username = user.UserName,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userRepository.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    var link = Url.Action(
                    "Contact",
                    "Home",
                    new { },
                    HttpContext.Request.Scheme,
                    HttpContext.Request.Host.Value);

                    _mailHelper.SendMail(model.Username, "Confirmação de reposição de palavra-passe.",
                    $"<h1>Confirmação de reposição de palavra-passe.</h1>" +
                    $"A sua palavra-passe foi reposta com sucesso.</br>" +
                    $"Se não foi você que repos a sua palavra-passe contacte-nos através do seguinte <a href = \"{link}\">link</a>.");
                    ViewBag.Message = "Palavra-passe reposta com sucesso.";
                    return View();
                }

                ViewBag.Message = $"Erro na reposição da palavra-passe. ({result.Errors.ToList()[0].Description})"; //get's the first error in the error list
                return View(model);
            }

            ViewBag.Message = "Utilizador não encontrado.";
            return View(model);
        }





        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userRepository.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction(nameof(Details));
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User Not Found.");
                }
            }

            return View(model);
        }


        #endregion


        #region Ajax Requests
        public async Task<JsonResult> GetRoomsAsync(int specialityId)
        {
            var rooms = await _comboHelper.GetComboRoomsAsync(specialityId);
            return this.Json(rooms);
        }

        #endregion

    }
}
