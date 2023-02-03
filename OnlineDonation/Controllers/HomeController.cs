using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineDonation.Data;
using OnlineDonation.Data.Entity;
using OnlineDonation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineDonation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<WebAppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _context.Users.ToList();
            if (users.Count() < 1)
            {
                await createAdmin();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (user.IsAdmin)
                {
                    return RedirectToAction("AdminView");
                }
            }
            return View();
        }
        public IActionResult AdminView()
        {
            return View();
        }
        public IActionResult UserView()
        {
            return View();
        }

        private async Task<IActionResult> createAdmin()
        {
            try
            {
                var date = DateTime.Now.AddYears(-21);
                var user = new WebAppUser { Name = "Admin User", UserName = "Onlinedonationtsu01@yopmail.com", EmailConfirmed = true, Email = "Onlinedonationtsu01@yopmail.com", IsAdmin = true };
                var result = await _userManager.CreateAsync(user, "Onlinedonationtsu01@");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
