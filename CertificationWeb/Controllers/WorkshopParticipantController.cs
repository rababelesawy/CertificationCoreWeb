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


namespace CertificationCoreWeb.Controllers
{
    public class WorkshopParticipantController : BaseController
    {
        private readonly Context _db;

        public WorkshopParticipantController(Context Db):base(Db) 
        {
            _db = Db;
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



        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0 && file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfRow = workSheet.Dimension.End.Row;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var Obj = new WorkshopParticipant();

                        Obj.Name = workSheet.Cells[rowIterator, 1]?.Value?.ToString();
                        Obj.Email = workSheet.Cells[rowIterator, 2]?.Value?.ToString();
                        Obj.Phone = workSheet.Cells[rowIterator, 3]?.Value?.ToString();

                        Obj.IsPrinted = false;
                        Obj.IsEmailSended = false;
                        Obj.CourseId = int.Parse(Request.Form["CourseId"]);  // Get the CourseId from a hidden field or parameter

                        _db.WorkshopParticipants.Add(Obj);
                        await _db.SaveChangesAsync();
                    }
                }

                return Json("1");  // Success
            }

            return Json("-1");  // Failure
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