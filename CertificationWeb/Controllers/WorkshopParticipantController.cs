using Certification.Domain.Entities;
using Certification.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using CertificationWeb.Controllers;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace CertificationCoreWeb.Controllers
{
    public class WorkshopParticipantController : BaseController
    {
        private readonly Context _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<WorkshopParticipantController> _logger;
        private readonly IConfiguration _configuration;

        public WorkshopParticipantController(Context Db, UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ILogger<WorkshopParticipantController> logger , IConfiguration configuration) :base(Db) 
        {
            _db = Db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
        }
        // GET: WorkshopParticipant
        public ActionResult Index()
        {

            return View(_db.WorkshopParticipants.ToList());
        }


        [HttpGet]
        public async Task<IActionResult> DownloadFile()
        {
          
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Content", "exceltampletes", "workshopparticipant.xlsx");

            if (!System.IO.File.Exists(path))
            {
                return NotFound(); // Return 404 if the file doesn't exist
            }

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(path);
            string fileName = "workshopparticipant.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }





        //[HttpPost]
        //public async Task<IActionResult> Upload(IFormFile file)
        //{
        //    try
        //    {
        //        // Set the EPPlus license context (this should be set before using ExcelPackage)
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //        if (file != null && file.Length > 0 && file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //        {
        //            using (var package = new ExcelPackage(file.OpenReadStream()))
        //            {
        //                var workbook = package.Workbook;


        //                if (workbook.Worksheets.Count == 0)
        //                {
        //                    return Json("-1");  
        //                }

        //                var workSheet = workbook.Worksheets.First();  

        //                // Ensure the worksheet has rows
        //                if (workSheet.Dimension == null)
        //                {
        //                    return Json("-1");  
        //                }

        //                var noOfRow = workSheet.Dimension.End.Row;

        //                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
        //                {
        //                    var Obj = new WorkshopParticipant();

        //                    Obj.Name = workSheet.Cells[rowIterator, 1]?.Value?.ToString();
        //                    Obj.Email = workSheet.Cells[rowIterator, 2]?.Value?.ToString();
        //                    Obj.Phone = workSheet.Cells[rowIterator, 3]?.Value?.ToString();

        //                    Obj.IsPrinted = false;
        //                    Obj.IsEmailSended = false;
        //                    Obj.CourseId = int.Parse(Request.Form["CourseId"]);  // Get the CourseId from the form

        //                    _db.WorkshopParticipants.Add(Obj);
        //                    await _db.SaveChangesAsync();
        //                }
        //            }

        //            return Json("1");  
        //        }
        //        else
        //        {
        //            return Json("-1");  
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.Message);
        //        return Json("-1");  
        //    }
        //}





        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var currentUser = await _userManager.GetUserAsync(User);
                bool isFreeUser = await _userManager.IsInRoleAsync(currentUser, "FreeUser");

                int freeUserLimit = _configuration.GetValue<int>("ParticipantSettings:FreeUserLimit");
                int maxAllowedParticipants = isFreeUser ? freeUserLimit : _configuration.GetValue<int>("ParticipantSettings:MaxAllowedParticipants");

                int courseId = int.Parse(Request.Form["CourseId"]);

                int currentParticipantsCount = _db.WorkshopParticipants
                    .Count(p => p.CourseId == courseId);

             
                bool isCourseCreator = await _db.Courses
                    .AnyAsync(c => c.CourseId == courseId && c.CreatedBy == currentUser.Id);

               
                int applicableLimit = isFreeUser && isCourseCreator ? 5 : maxAllowedParticipants;

          
                if (currentParticipantsCount >= applicableLimit)
                {
                    return Json(isFreeUser ? "FreeUserParticipantLimitReached" : $"{applicableLimit} Only");
                }

                int availableSlots = applicableLimit - currentParticipantsCount;

                if (file != null && file.Length > 0 && file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    using (var package = new ExcelPackage(file.OpenReadStream()))
                    {
                        var workbook = package.Workbook;

                        if (workbook.Worksheets.Count == 0)
                        {
                            return Json("-1");
                        }

                        var workSheet = workbook.Worksheets.First();

                        if (workSheet.Dimension == null)
                        {
                            return Json("-1");
                        }

                        var noOfRow = workSheet.Dimension.End.Row;
                        var participants = new List<WorkshopParticipant>();

                        for (int rowIterator = 2; rowIterator <= noOfRow && participants.Count < availableSlots; rowIterator++)
                        {
                            var participant = new WorkshopParticipant
                            {
                                Name = workSheet.Cells[rowIterator, 1]?.Value?.ToString(),
                                Email = workSheet.Cells[rowIterator, 2]?.Value?.ToString(),
                                Phone = workSheet.Cells[rowIterator, 3]?.Value?.ToString(),
                                IsPrinted = false,
                                IsEmailSended = false,
                                CourseId = courseId,
                            };

                            if (!string.IsNullOrEmpty(participant.Name) && !string.IsNullOrEmpty(participant.Email))
                            {
                                participants.Add(participant);
                            }
                        }

                        if (participants.Count > 0)
                        {
                            _db.WorkshopParticipants.AddRange(participants);
                            await _db.SaveChangesAsync();

                            if (noOfRow > availableSlots + 1)
                            {
                                return Json(isFreeUser ? "FreeUserParticipantLimitReached" : $"{maxAllowedParticipants} Only");
                            }
                            return Json("1");
                        }

                        return Json("-1");
                    }
                }
                else
                {
                    return Json("-1");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while uploading participants");
                return Json("-1");
            }
        }




        [HttpPost]
        public IActionResult ActiveCertification(Guid workshopParticipantId)
        {
            var workshopParticipant = _db.WorkshopParticipants.Find(workshopParticipantId);

            if (workshopParticipant == null)
            {
                return Json("-1"); // Return error if participant not found
            }

            try
            {
                var serverVal = Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "CertificationEmail.html");

                BackgroundJob.Enqueue(() => SendEmail(
                    $"شهادة حضور ورشة عمل {workshopParticipant.Course.CourseName}",
                    new List<WorkshopParticipant> { workshopParticipant },
                    serverVal,
                    workshopParticipant.Course.CourseName,
                    workshopParticipant.Course.CourseDate,
                    workshopParticipant.Course.CoachName));

            }
            catch (Exception)
            {
                return Json("-1");
            }

            return Json("1");
        }

        ////[HttpPost]
        ////public async Task<IActionResult> SendWhatsApp(Guid workshopParticipantId)
        ////{
        ////    string from = "01024542216";
        ////    string to = "01060242617"; // Sender Mobile
        ////    string msg = "نشكركم على مشاركتكم المتميزة في حضور";

        ////    WhatsApp wa = new WhatsApp(from, "BnXk*******B0=", "NickName", true, true);
        ////    string message = "";

        ////    try
        ////    {
        ////        await Task.Run(() =>
        ////        {
        ////            wa.OnConnectSuccess += () =>
        ////            {
        ////                message = "Connected to WhatsApp...";

        ////                wa.OnLoginSuccess += (phoneNumber, data) =>
        ////                {
        ////                    wa.SendMessage(to, msg);
        ////                    message = "Message Sent...";
        ////                };

        ////                wa.OnLoginFailed += (data) =>
        ////                {
        ////                    message = $"Login Failed : {data}";
        ////                };

        ////                wa.Login();
        ////            };

        ////            wa.OnConnectFailed += (ex) =>
        ////            {
        ////                message = "Connection Failed...";
        ////            };

        ////            wa.Connect();
        ////        });
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        message = $"Error: {ex.Message}";
        ////    }

        ////    return Json(message);
        ////}
    }

    }
