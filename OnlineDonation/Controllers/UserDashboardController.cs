using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineDonation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineDonation.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public int GetUsersCount()
        {
            return _context.Users.Where(x => !x.IsAdmin && x.EmailConfirmed).Count();
        }

        public int GetFundsCreatedCount()
        {
            return _context.Donations.Count();
        }

        public int GetVisitorsCount()
        {
            return _context.Visitors.Count();
        }

        public List<string> GetMonths() 
        {
            var lastSixMonths = Enumerable.Range(1, 6)
                                  .Select(i => DateTime.Now.AddMonths(i - 6))
                                  .Select(date => date.ToString("MMMM"));
            return lastSixMonths.ToList();
        }

        public List<int> GetVisitorsData()
        {
            var visitorData = new List<int>();
            var lastSixMonths = Enumerable.Range(1, 6)
                                  .Select(i => DateTime.Now.AddMonths(i - 6))
                                  .Select(date => date.Month).ToList();
            foreach (var month in lastSixMonths)
            {
                var data = _context.Visitors.Where(x => x.DateTime.Month == month).Count();
                visitorData.Add(data);
            }
            return visitorData;
        }

        public List<int> GetFundsData()
        {
            var fundsData = new List<int>();
            var lastSixMonths = Enumerable.Range(1, 6)
                                  .Select(i => DateTime.Now.AddMonths(i - 6))
                                  .Select(date => date.Month).ToList();
            foreach (var month in lastSixMonths)
            {
                var data = _context.Donations.Where(x => x.DateCreated.Month == month).Count();
                fundsData.Add(data);
            }
            return fundsData;
        }

        public List<List<object>> GetChartDonation(int itemId)
        {
            List<List<object>> array_list = new List<List<object>>();
            var transactions = _context.Transactions.Include(x => x.User).Where(x => x.DonationId == itemId).ToList();
            var goal = _context.Donations.Find(itemId);
            var strings = new List<object>();
            strings.Add("Donation");
            strings.Add("Amount");
            array_list.Add(strings);

            foreach (var transaction in transactions)
            {
                var result = new List<object>();
                result.Add(transaction.User.Name);
                result.Add(transaction.Amount);
                array_list.Add(result);
            }

            if (transactions.Sum(x => x.Amount) < goal.GoalAmount)
            {
                var result = new List<object>();
                result.Add("Remaining Amount");
                result.Add(goal.GoalAmount - transactions.Sum(x => x.Amount));
                array_list.Add(result);

            }
            return array_list;
        }

        public async Task<bool> SaveCurrentUserLocation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            user.AnsweredLocation = true;
            user.AllowLocation = true;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectCurrentUserLocation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            user.AnsweredLocation = true;
            user.AllowLocation = false;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
