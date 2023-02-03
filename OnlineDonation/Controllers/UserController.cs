using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineDonation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var user = _context.Users.Where(x => !x.IsAdmin).ToList();
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _context.Users.FindAsync(id);
            var donations = _context.Donations.Where(x => x.UserId == user.Id).ToList();
            var transactions = _context.Transactions.Where(x => x.UserId == user.Id).ToList();
            var visitors = _context.Visitors.Where(x => x.UserId == user.Id).ToList();
            _context.Users.Remove(user);
            foreach (var donation in donations)
                _context.Donations.Remove(donation);
            foreach (var transaction in transactions)
                _context.Transactions.Remove(transaction);
            foreach (var visitor in visitors)
                _context.Visitors.Remove(visitor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
