﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using The_Pag.Models;

namespace The_Pag.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        

        

        public IActionResult Index()
        {
            return View();
        }

        

        public IActionResult Item_Management()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Order_Details()
        {
            return View();
        }

        public IActionResult Order_History()
        {
            return View();
        }

        public IActionResult Update_Order_Details()
        {
            return View();
        }

        public IActionResult User_Management()
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