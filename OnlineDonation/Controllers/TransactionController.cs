using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineDonation.Data;
using OnlineDonation.Data.Entity;
using OnlineDonation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OnlineDonation.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebAppUser> _userManager;
        private readonly IEmailSender _emailSender;
        public TransactionController(ApplicationDbContext context,
            UserManager<WebAppUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            ViewBag.IsAdmin = false;
            if (user.IsAdmin)
            {
                ViewBag.IsAdmin = true;
                return View(await _context.Transactions.Include(x => x.Donation).ToListAsync());
            }

            else
                return View(await _context.Transactions.Include(x => x.Donation).Where(x => x.UserId == user.Id).ToListAsync());
        }

        public async Task<IActionResult> ViewTransactionByDonationId(int id)
        {
            return View(await _context.Transactions.Where(x => x.DonationId == id).Include(x => x.User).ToListAsync());
        }

        public async Task<bool> SaveTransaction(string userId, int amount, long referenceCode, int itemId)
        {
            Transaction transaction = new Transaction()
            {
                UserId = userId,
                Amount = amount,
                ReferenceCode = referenceCode,
                DateTime = DateTime.Now,
                DonationId = itemId
            };
            _context.Add(transaction);
            await _context.SaveChangesAsync();


            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            var donation = _context.Donations.Where(x => x.Id == itemId).FirstOrDefault();
            if (user != null && donation != null)
            {
                var userName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Email")["UserName"];
                await _emailSender.SendEmailAsync(userName, "Transaction Receipt " + DateTime.Now,
                "Here is the transaction details:" +
                "<br>" +
                "Donor: <b>" + user.Name + "</b>" +
                "<br>" +
                "Amount: <b>" + amount + "</b>" +
                "<br>" +
                "Reference Code: <b>" + referenceCode + "</b>" +
                "<br>" +
                "Date Donated: <b>" + DateTime.Now + "</b>" +
                "<br>" +
                "Donation Detail: <b>" + donation.Title + "</b>"
                );
            }

            return true;
        }

        public int GetTransactionsAmount(int itemId)
        {
            return _context.Transactions.Where(x => x.DonationId == itemId).Sum(x => x.Amount);
        }

        public int GetTransactionsCount(int itemId)
        {
            return _context.Transactions.Where(x => x.DonationId == itemId).Count();
        }

        public IActionResult GenerateReport()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transactions");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Donor";
                worksheet.Cell(currentRow, 2).Value = "Donation Title";
                worksheet.Cell(currentRow, 3).Value = "Amount";
                worksheet.Cell(currentRow, 4).Value = "Reference Code";
                worksheet.Cell(currentRow, 5).Value = "Date Time";

                var transactions = _context.Transactions.Include(x => x.User).Include(x => x.Donation).ToList();

                foreach (var transaction in transactions)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = transaction.User.Name;
                    worksheet.Cell(currentRow, 2).Value = transaction.Donation.Title;
                    worksheet.Cell(currentRow, 3).Value = transaction.Amount;
                    worksheet.Column(4).CellsUsed().SetDataType(XLDataType.Text);
                    worksheet.Cell(currentRow, 4).Value = transaction.ReferenceCode.ToString();
                    worksheet.Cell(currentRow, 5).Value = transaction.DateTime;
                }
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Transactions_" + DateTime.Now.ToString("MM/dd/YYYY") + ".xlsx"
                    );
                }
            }
        }
    }
}
