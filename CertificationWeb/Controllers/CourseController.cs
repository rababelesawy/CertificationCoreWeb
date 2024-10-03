using Certification.Domain.Entities;
using Certification.Infrastructure.Data;

using X.PagedList;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Hangfire;

using Certification.Domain.DomainModels;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using CertificationWeb.Controllers;

namespace CertificationCoreWeb.Controllers
{
    [Authorize]
    public class CourseController : BaseController
    { 
        private readonly Context _dB;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CourseController(Context DB , IWebHostEnvironment webHostEnvironment) :base(DB)
        {
            _dB = DB;
           _webHostEnvironment = webHostEnvironment;
        }

        
  

        

        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
 // Ensure this is included at the top of your file

    public ActionResult CourseList(int itemsPerPage = 10, int currentPage = 1)
    {
        ViewBag.Theme = "custom";
        ViewBag.Type = 1;
        ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

        // Fetch courses created by the current user and paginate
        var coursesQuery = _dB.Courses
            .Where(x => x.CreatedBy == CurrentUser.UserId)
            .OrderByDescending(x => x.CourseId);

        // Get the total count for pagination
        var totalItemCount = coursesQuery.Count();

        // Get the paginated result
        var courses = coursesQuery
            .ToPagedList(currentPage, itemsPerPage); // Convert to IPagedList

        // Create a model to hold the paginated data
        var model = new PagedResultViewModel<Certification.Domain.Entities.Course>
        {
            Items = courses, // Items to be passed to the view
            ItemsPerPage = itemsPerPage,
            TotalCount = totalItemCount // Total count for pagination
        };

        return PartialView("_CourseList", model);
    }



    public IActionResult AddCourse(int id = 0)
        {
            Course model = new Course
            {
                CertificationImage = 0,
                CertificationName = "قم بتحريك النص لتعديل وضعه بالشهادة",
                CreatedBy = CurrentUser.UserId 
            };

            if (id != 0)
            {
                model = _dB.Courses.Find(id);
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken] // Security improvement in ASP.NET Core
        public IActionResult SaveCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                if (course.CourseId == 0)
                {
                    course.CreationDate = DateTime.Now;
                    _dB.Courses.Add(course);
                }
                else
                {
                    _dB.Entry(course).State = EntityState.Modified;
                }

                _dB.SaveChanges();
                return Json("1");
            }

            return BadRequest("Invalid model");
        }





        public IActionResult ActiveCertifications(int CourseId)
        {
            var course = _dB.Courses.Find(CourseId); // Assuming _db is injected
            var workshopParticipants = _dB.WorkshopParticipants
                .AsNoTracking()
                .Where(x => x.CourseId == CourseId && !x.IsEmailSended)
                .ToList();

            try
            {
                // Get the path to the CertificationEmail.html template
                var serverVal = Path.Combine(_webHostEnvironment.WebRootPath, "Views", "Shared", "CertificationEmail.html");

                // Queue the email sending process using Hangfire
                BackgroundJob.Enqueue(() => SendEmail(
                    string.Concat("شهادة حضور ورشة عمل ", course.CourseName),
                    workshopParticipants,
                    serverVal,
                    course.CourseName,
                    course.CourseDate,
                    course.CoachName)
                );
            }
            catch (Exception e)
            {
                return Json("-1"); // Returning error response
            }

            return Json("1"); // Return success response
        }



    }
}