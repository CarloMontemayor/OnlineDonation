using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ShareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShareController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Shares.ToListAsync());
        }

        public async Task<bool> SaveShare(int donationId)
        {
            Share visitor = new Share()
            {
                DonationId = donationId,
                DateTime = DateTime.Now
            };
            _context.Add(visitor);
            await _context.SaveChangesAsync();
            return true;
        }

        public int GetSharesCount(int itemId)
        {
            return _context.Shares.Where(x => x.DonationId == itemId).Count();
        }
    }
}
