using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineDonation.Data;
using OnlineDonation.Data.Entity;
using OnlineDonation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineDonation.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public DonationController(ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.Where(x => x.Id == userId).FirstOrDefault();
            var donationModel = _db.Donations.Where(x => x.Status == Data.Enum.Status.Approved).OrderBy(x => x.CategoryId == user.CategoryId).ToList();
            return View(donationModel);
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            var model = new DonationViewModel();
            ViewBag.Category = new SelectList(_db.Categories, "CategoryId", "Name");
            if (id != 0)
            {
                var donationModel = _db.Donations.SingleOrDefault(x => x.Id == id);
                //model = _mapper.Map<DonationViewModel>(donationModel);
                model.Id = donationModel.Id;
                model.GoalAmount = donationModel.GoalAmount;
                model.RaisedMoneyFor = donationModel.RaisedMoneyFor;
                model.Title = donationModel.Title;
                model.Description = donationModel.Description;
                model.ImagePathString = donationModel.ImagePath;
                model.QRImagePathString = donationModel.QRImagePath;
                model.ClosingDate = donationModel.ClosingDate;
                model.Status = donationModel.Status;
            }
            model.ClosingDate = DateTime.Now.AddMonths(1);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(DonationViewModel model)
        {
            if (model.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    string imageFileName = UploadedFile(model);
                    string qRimageFileName = QRUploadedFile(model);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Donation donationModel = new Donation
                    {
                        GoalAmount = model.GoalAmount,
                        RaisedMoneyFor = model.RaisedMoneyFor,
                        Title = model.Title,
                        Description = model.Description,
                        ImagePath = imageFileName,
                        QRImagePath = qRimageFileName,
                        UserId = userId,
                        DateCreated = DateTime.Now,
                        ClosingDate = model.ClosingDate,
                        CategoryId = model.CategoryId,
                        Status = Data.Enum.Status.Pending,
                    };
                    _db.Add(donationModel);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = true;
                    return RedirectToAction("MyFundRaisers");
                }
            }
            else
            {
                if (model.ImagePath == null && model.QRImagePath == null)
                {
                    var donationDb = _db.Donations.AsNoTracking().SingleOrDefault(x => x.Id == model.Id);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Donation donationModel = new Donation
                    {
                        Id = model.Id,
                        GoalAmount = model.GoalAmount,
                        RaisedMoneyFor = model.RaisedMoneyFor,
                        Title = model.Title,
                        Description = model.Description,
                        ImagePath = donationDb.ImagePath,
                        QRImagePath = donationDb.QRImagePath,
                        CategoryId = donationDb.CategoryId,
                        UserId = userId,
                        Status = donationDb.Status
                    };
                    _db.Update(donationModel);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = true;
                    return RedirectToAction("MyFundRaisers");
                }
                else if (model.ImagePath == null && model.QRImagePath != null)
                {
                    var donationDb = _db.Donations.AsNoTracking().SingleOrDefault(x => x.Id == model.Id);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    string qRimageFileName = QRUploadedFile(model);
                    Donation donationModel = new Donation
                    {
                        Id = model.Id,
                        GoalAmount = model.GoalAmount,
                        RaisedMoneyFor = model.RaisedMoneyFor,
                        Title = model.Title,
                        Description = model.Description,
                        ImagePath = donationDb.ImagePath,
                        QRImagePath = qRimageFileName,
                        CategoryId = donationDb.CategoryId,
                        UserId = userId,
                        Status = donationDb.Status
                    };
                    _db.Update(donationModel);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = true;
                    return RedirectToAction("MyFundRaisers");
                }
                else
                {
                    string imageFileName = UploadedFile(model);
                    string qRimageFileName = QRUploadedFile(model);
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Donation donationModel = new Donation
                    {
                        Id = model.Id,
                        GoalAmount = model.GoalAmount,
                        RaisedMoneyFor = model.RaisedMoneyFor,
                        Title = model.Title,
                        Description = model.Description,
                        ImagePath = imageFileName,
                        QRImagePath = qRimageFileName,
                        CategoryId = model.CategoryId,
                        UserId = userId,
                        Status = model.Status
                    };
                    _db.Update(donationModel);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = true;
                    return RedirectToAction("MyFundRaisers");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DonationViewModel model)
        {
            var donation = _db.Donations.Find(model.Id);
            if (donation != null)
            {
                if (model.CustomFile != null)
                {
                    string customFileName = CustomUploadedFile(model);
                    donation.CustomImagePath = customFileName;
                    _db.Update(donation);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = true;
                    return RedirectToAction("MyFundRaisers");
                }
            }
            return RedirectToAction("ViewFundRaiser", new { id = model.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var events = await _db.Donations.FindAsync(id);
            _db.Donations.Remove(events);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string UploadedFile(DonationViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImagePath != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private string QRUploadedFile(DonationViewModel model)
        {
            string uniqueFileName = null;

            if (model.QRImagePath != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.QRImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.QRImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private string CustomUploadedFile(DonationViewModel model)
        {
            string uniqueFileName = null;

            if (model.CustomFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CustomFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CustomFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult MyFundRaisers()
        {
            ViewBag.Success = TempData["Success"];
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var donationModel = _db.Donations.Where(x => x.UserId == userId).ToList();
            return View(donationModel);
        }

        public IActionResult ViewFundRaiser(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.Where(x => x.Id == userId).FirstOrDefault();
            var donationModel = _db.Donations.SingleOrDefault(x => x.Id == id);
            ViewBag.Userid = user.Id;
            return View(donationModel);
        }

        public IActionResult AdminView()
        {
            var donationModel = _db.Donations.Include(x => x.Category).ToList();
            return View(donationModel);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var donationModel = _db.Donations.Where(x => x.Id == id).FirstOrDefault();
            if (donationModel != null)
            {
                donationModel.Status = Data.Enum.Status.Approved;
                _db.Update(donationModel);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminView));
        }

        public async Task<IActionResult> Reject(int id)
        {
            var donationModel = _db.Donations.Where(x => x.Id == id).FirstOrDefault();
            if (donationModel != null)
            {
                donationModel.Status = Data.Enum.Status.Rejected;
                _db.Update(donationModel);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminView));
        }
    }
}
