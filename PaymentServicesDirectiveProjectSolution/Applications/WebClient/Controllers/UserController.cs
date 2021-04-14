using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ApplicationService;
using WebClient.Models.ViewModels;

namespace WebClient.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserVM model)
        {
            await _userService.CreateUser(model.FirstName, model.LastName, model.PersonalNumber, model.BankName,
                model.BankAccountNumber, model.BankPinNumber);

            return RedirectToAction("Index", "Home");
        }
    }
}
