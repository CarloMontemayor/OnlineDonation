using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    public class VisitorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;

        public VisitorController(ApplicationDbContext context,
            UserManager<WebAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Visitors.ToListAsync());
        }

        public async Task<bool> SaveVisitor(string ip, string latitude, string longitude)
        {
            var userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            Visitor visitor = new Visitor()
            {
                VisitorIp = ip,
                DateTime = DateTime.Now,
                UserId = userId,
                Latitude = latitude,
                Longitude = longitude
            };
            _context.Add(visitor);
            await _context.SaveChangesAsync();
            return true;
        }

        public string GetUserIp(string userId)
        {
            var visitor = _context.Visitors.Where(x => x.UserId == userId).OrderByDescending(x => x.DateTime).FirstOrDefault();
            if (visitor != null)
            {
                //Create my object
                var myData = new
                {
                    Latitude = visitor.Latitude,
                    Longitude = visitor.Longitude
                };

                //Tranform it to Json object
                return JsonConvert.SerializeObject(myData);
            }
            return "";
        }
    }
}
