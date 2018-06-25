using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class UserRegistrationController : Controller
    {
        private IUserService _userService;
        public UserRegistrationController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.RegisterUser(userModel);
                //return Content($"User {userModel.FirstName} {userModel.LastName} has been registered successfully");

                return RedirectToAction(nameof(EmailConfirmation), new { userModel.Email });
            }

            return View(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user?.IsEmailConfirmed == true)
                return RedirectToAction("Index", "GameInvitation", new { email = email });

            ViewBag.Email = email;

            // This code is no longer needed here.
            // Communication Middleware is now going to simulate the effective email confirmation
            //user.IsEmailConfirmed = true;
            //user.EmailConfirmationDate = DateTime.Now;
            //await _userService.UpdateUser(user);

            return View();
        }
    }
}