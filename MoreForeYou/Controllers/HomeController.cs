using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoreForeYou.Models;
using MoreForYou.Services.Contracts.Email;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoreForeYou.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMGraphMailService _mGraphMailService;

        public HomeController(ILogger<HomeController> logger, IMGraphMailService mGraphMailService)
        {
            _logger = logger;
            _mGraphMailService = mGraphMailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> SendMail()
        {
            var x =  _mGraphMailService.sendTest();
            return x.Status.ToString();
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
