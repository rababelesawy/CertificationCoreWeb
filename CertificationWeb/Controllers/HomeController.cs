using Certification.Domain.DomainModels;
using Certification.Domain.Entities;
using Certification.Infrastructure.Data;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using CertificationWeb.Controllers;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;
using Microsoft.AspNetCore.Http;


namespace CertificationWeb.Controllers
{

    //[Authorize]
    public class HomeController :BaseController
    {
        private readonly Context _dB;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(Context DB , IHttpContextAccessor httpContextAccessor):base(DB)
        {
            _dB = DB;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var courses = _dB.Courses.ToList();

            // Create a list of SelectListItems
            ViewBag.Courses = courses.Select(s =>
                new SelectListItem
                {
                    Text = s.CourseName,
                    Value = s.CourseId.ToString()
                }).ToList();

            // Initialize the WorkshopParticipantModel
            var model = new WorkshopParticipantModel
            {
                WorkshopParticipantSearchModel = new WorkshopParticipantSearchModel() // Ensure this is initialized
            };

            return View(model);

        }
        public IActionResult WorkshopParticipants(int itemsPerPage = 10, int currentPage = 1)
        {
            ViewBag.Theme = "custom";
            ViewBag.Type = 1;
            ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

            // Get current user id
            var currentUserId = CurrentUser?.UserId;

            if (currentUserId == null)
            {
                // Handle case when user is not authenticated
                return NotFound(); // Or redirect to an appropriate action
            }

            // Get workshop participants
            var workshopParticipants = _dB.WorkshopParticipants
                .Where(x => x.Course.CreatedBy == currentUserId)
                .ToList();

            var totalUserCount = workshopParticipants.Count;
            var printUserCount = workshopParticipants.Count(x => x.IsPrinted == true);
            var nonPrintUserCount = workshopParticipants.Count(x => x.IsPrinted != true);

            var pagedWorkshopParticipants = workshopParticipants
                .OrderByDescending(x => x.CourseId)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            var result = new WorkshopParticipantModel
            {
                WorkshopParticipantSearchModel = new WorkshopParticipantSearchModel(),
                WorkshopParticipants = new PagedResultViewModel<WorkshopParticipant>
                {
                    Items = (IPagedList<WorkshopParticipant>)pagedWorkshopParticipants,
                    TotalCount = totalUserCount,
                    CurrentPage = currentPage,
                    ItemsPerPage = itemsPerPage
                },
                TotalUser = totalUserCount,
                PrintUser = printUserCount,
                NonPrintUser = nonPrintUserCount
            };

            return PartialView("_WorkshopParticipantList", result);
        }












        //[HttpPost]
        //    public ActionResult WorkshopParticipantSearch(WorkshopParticipantSearchModel model,int itemsPerPage = 10, int currentPage = 1)
        //    {
        //        ViewBag.Theme = "custom";
        //        ViewBag.index = itemsPerPage * (currentPage - 1) + 1;
        //        var WorkshopParticipants = _dB.WorkshopParticipants.Where(x => x.Course.CreatedBy == CurrentUser.UserId && (model.Name == null || x.Name.Contains(model.Name)) &&
        //             (model.Phone == null || x.Phone == model.Phone)  &&(model.CourseId==0||x.CourseId==model.CourseId)&&
        //             (model.CoachName==null||x.Course.CoachName.Contains(model.CoachName))).ToList();

        //        WorkshopParticipantModel res = new WorkshopParticipantModel
        //        {
        //            WorkshopParticipantSearchModel = model,
        //            WorkshopParticipants = WorkshopParticipants.Where(x => x.Course.CreatedBy == CurrentUser.UserId && (model.IsPrinted == null || x.IsPrinted == model.IsPrinted)&& (model.IsSended == null || x.IsEmailSended == model.IsSended)).OrderByDescending(x =>x.CourseId /*x.WorkshopParticipantId*/).AsQueryable().Page(currentPage),
        //            NonPrintUser= WorkshopParticipants.Where(x=> x.Course.CreatedBy == CurrentUser.UserId && x.IsPrinted!=true).Count(),
        //            TotalUser= WorkshopParticipants.Count(),
        //            PrintUser= WorkshopParticipants.Where(x => x.Course.CreatedBy == CurrentUser.UserId && x.IsPrinted == true).Count()
        //        };

        //        return PartialView("_WorkshopParticipantList", res);
        //    }

        [AllowAnonymous]
        public ActionResult EditUserInfo(Guid Id)
        {
            var model = _dB.WorkshopParticipants.Where(x => x.WorkshopParticipantId == Id).FirstOrDefault();
            if(model==null)
                return View("ErrorPage");
            if (model.IsPrinted==true)
                return RedirectToAction("UserCertificate", new { Id=model.WorkshopParticipantId });
            else
            return View("UserInfo", model);
        }


        [AllowAnonymous]
        public ActionResult UserInfo(Guid Id)
        {  
                return View("ErrorPage");
        }

        [AllowAnonymous]
        public ActionResult UserCertificate(Guid Id)
        {
            var model = _dB.WorkshopParticipants.Where(x => x.WorkshopParticipantId == Id).FirstOrDefault();
            return View(model);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserInfo(WorkshopParticipant model )
        {
            var WorkshopParticipant = _dB.WorkshopParticipants.Find(model.WorkshopParticipantId);
            WorkshopParticipant.Name = model.Name;
            WorkshopParticipant.Phone = model.Phone;
            WorkshopParticipant.Email = model.Email;
            _dB.SaveChanges();
            return Json("1");
        }


        [AllowAnonymous]
        public ActionResult GetCertification(Guid Id)
        {
            //CourseParticipationModel model = new CourseParticipationModel { workshopParticipant= };
            //db.WorkshopParticipants.Where(x => x.WorkshopParticipantId == Id).FirstOrDefault();

            return View("Certification", _dB.WorkshopParticipants.Where(x => x.WorkshopParticipantId == Id).FirstOrDefault());
        }


        [AllowAnonymous]
        public async Task<IActionResult> PrintCertification(Guid id)
        {
            // Retrieve the participant along with the course information
            var model = await _dB.WorkshopParticipants
                .Include(wp => wp.Course)
                .FirstOrDefaultAsync(x => x.WorkshopParticipantId == id);

            // Check if the model is null
            if (model == null)
            {
                return NotFound(); 
            }

           
            model.IsPrinted = true;

           
            await _dB.SaveChangesAsync();

            // Return the PDF document
            return new ViewAsPdf("Certification", model)
            {
                FileName = "Certification.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }


        [AllowAnonymous]
        public ActionResult Certification(WorkshopParticipant model)
        {
            var model2 = _dB.WorkshopParticipants
                .Include(wp => wp.Course) 
                .FirstOrDefault(x => x.WorkshopParticipantId == model.WorkshopParticipantId);

            if (model2 == null)
            {
               
                return NotFound(); 
            }

            return View("Certification", model2);
        }


    }
}