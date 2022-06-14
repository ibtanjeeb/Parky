﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accountRepo;
        public HomeController(ILogger<HomeController> logger,INationalParkRepository npRepo,ITrailRepository trailRepo,IAccountRepository accountRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _logger = logger;
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVm indexVm = new IndexVm()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath,HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailAPIPath, HttpContext.Session.GetString("JWToken"))

            };
            return View(indexVm);
        }

        public IActionResult Privacy()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            var obj = new User();
           

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accountRepo.LogInAsync(SD.AccountAPIPath+"authenticate/", obj);
            if(objUser.Token==null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            HttpContext.Session.SetString("JWToken",objUser.Token);

            return RedirectToAction("Index");


          
        }
        [HttpGet]
        public IActionResult Register()
        {
            


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool objUser = await _accountRepo.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (objUser == false)
            {
                return View();
            }
           

            return RedirectToAction("Login");



        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            
             HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");



        }
        [HttpGet]
        public IActionResult AccessDenied()
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
