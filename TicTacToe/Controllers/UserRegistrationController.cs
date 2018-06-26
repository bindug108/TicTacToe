﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    public class UserRegistrationController : Controller
    {
        readonly IUserService _userService;
        readonly IEmailService _emailService;

        public UserRegistrationController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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
            var urlAction = new UrlActionContext
            {
                Action = "confirmEmail",
                Controller = "UserRegistration",
                Values = new { email },
                Protocol = Request.Scheme,
                Host = Request.Host.ToString() 
            };

            var message = $"Thank you for your registration on our web site, please cick here to confirm your email " + $"{Url.Action(urlAction)}";
            try
            {
                _emailService.SendEmail(email, "Tic-Tac-Toe Email Confirmation", message).Wait();
            }
            catch (Exception e)
            {

            }

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