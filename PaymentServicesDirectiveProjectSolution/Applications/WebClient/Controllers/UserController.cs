using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Applications.WebClient.Models.ViewModels.UserVMS;
using Core.ApplicationService;
using Domain.DTOs;
using WebClient.Helpers;

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
            UserDto user = await _userService.CreateUser(model.FirstName, model.LastName, model.PersonalNumber, model.BankName,
                model.BankAccountNumber, model.BankPinNumber);

            return RedirectToAction("RegistrationSuccess", "User", new { userPass  = user.UserPass});
        }

        public IActionResult RegistrationSuccess(string userPass)
        {
            ViewBag.UserPass = userPass;
            return View();
        }

        public IActionResult GetUserDetails()
        {
            UserDetailsVM model = new UserDetailsVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserDetails(UserDetailsVM model)
        {
            UserDto user = await _userService.GetUserByPersonalNumber(model.PersonalNumber,model.UserPass);
            model.User = user.ToUserVm();
            return View(model);
        }

        public IActionResult ChangeUserPass()
        {
            return View(new ChangeUserPassVM());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPass(ChangeUserPassVM model)
        {
            UserDto user = await _userService.ChangeUserPass(model.PersonalNumber, model.OldUserPass, model.NewUserPass);

            return RedirectToAction("ChangeUserPassSuccess", new {message = $"Uspesno ste promenili korisnicku lozinku! {user.PersonalNumber}"});
        }

        public IActionResult ChangeUserPassSuccess(string message)
        {
            ViewBag.Message = message;
            return View();
        }


        public IActionResult BlockUser()
        {
            return View(new BlockUserVM());
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(BlockUserVM model)
        {
            await _userService.BlockUser(model.PersonalNumber, model.AdminPass, AdminHelper.AdminPass);

            return RedirectToAction("BlockUserSuccess", new {message = $"Uspesno ste blokirali korisnika! {model.PersonalNumber}"});
        }

        public IActionResult UnblockUser()
        {
            return View(new BlockUserVM());
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(BlockUserVM model)
        {
            await _userService.UnblockUser(model.PersonalNumber, model.AdminPass, AdminHelper.AdminPass);

            return RedirectToAction("BlockUserSuccess", new { message = $"Uspesno ste odblokirali korisnika! {model.PersonalNumber}" });
        }

        public IActionResult BlockUserSuccess(string message)
        {
            ViewBag.Message = message;
            return View();
        }

    }
}
