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

using CertificationWeb.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace CertificationCoreWeb.Controllers
{
    //[Authorize]
    public class CourseController : BaseController
    {
        private readonly Context _dB;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CourseController(Context DB, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IBackgroundJobClient backgroundJobClient) : base(DB)
        {
            _dB = DB;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _backgroundJobClient = backgroundJobClient;
        }


        //GET: Course
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
           
            var coursesQuery = _dB.Courses
                .OrderByDescending(x => x.CourseId); 

      
            var totalItemCount = coursesQuery.Count();

           
            var pagedCourses = coursesQuery
                .ToPagedList(page, pageSize); 

            var model = new PagedResultViewModel<Certification.Domain.Entities.Course>
            {
                Items = pagedCourses, 
                TotalCount = totalItemCount, 
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            return View(model);
        }





        #region CourseList old

        // Ensure this is included at the top of your file

        //public ActionResult CourseList(int itemsPerPage = 10, int currentPage = 1)
        //{
        //    ViewBag.Theme = "custom";
        //    ViewBag.Type = 1;
        //    ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

        //    // Fetch courses created by the current user and paginate
        //    var coursesQuery = _dB.Courses
        //        .Where(x => x.CreatedBy == CurrentUser.UserId)
        //        .OrderByDescending(x => x.CourseId);

        //    // Get the total count for pagination
        //    var totalItemCount = coursesQuery.Count();

        //    // Get the paginated result
        //    var courses = coursesQuery
        //        .ToPagedList(currentPage, itemsPerPage); // Convert to IPagedList

        //    // Create a model to hold the paginated data
        //    var model = new PagedResultViewModel<Certification.Domain.Entities.Course>
        //    {
        //        Items = courses, // Items to be passed to the view
        //        ItemsPerPage = itemsPerPage,
        //        TotalCount = totalItemCount // Total count for pagination
        //    };

        //    return PartialView("_CourseList", model);
        //}


        #endregion



        #region CourseList New
        public async Task<IActionResult> CourseList(int itemsPerPage = 10, int currentPage = 1)
        {
            ViewBag.Theme = "custom";
            ViewBag.Type = 1;
            ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var currentUserId = user.Id;

            var coursesQuery = _dB.Courses
                .Where(x => x.CreatedBy == currentUserId)
                .OrderByDescending(x => x.CourseId);

       
            var totalItemCount = await coursesQuery.CountAsync();

            
            var pagedCourses = await coursesQuery
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(); 

        
            var model = new PagedResultViewModel<Certification.Domain.Entities.Course>
            {
                Items = pagedCourses.ToPagedList(currentPage, itemsPerPage), 
                TotalCount = totalItemCount 
            };

            return PartialView("_CourseList", model);
        }



        #endregion


        #region AddAndSave Old

        //public async Task<IActionResult> AddCourse(int id = 0)
        //{

        //    var user = await _userManager.GetUserAsync(User);

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var currentUserId = user.Id;


        //    Course model = new Course
        //    {
        //        CertificationImage = 0,
        //        CertificationName = "قم بتحريك النص لتعديل وضعه بالشهادة",
        //        CreatedBy = currentUserId 
        //    };

        //    if (id != 0)
        //    {
        //        model = _dB.Courses.Find(id);
        //    }

        //    return View(model);
        //}




        //[HttpPost]
        //[ValidateAntiForgeryToken] 
        //public IActionResult SaveCourse(Course course)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (course.CourseId == 0)
        //        {
        //            course.CreationDate = DateTime.Now;
        //            _dB.Courses.Add(course);
        //        }
        //        else
        //        {
        //            _dB.Entry(course).State = EntityState.Modified;
        //        }

        //        _dB.SaveChanges();
        //        return Json("1");
        //    }

        //    return BadRequest("Invalid model");
        //}

        #endregion

        [HttpGet]
        public async Task<IActionResult> AddCourse(int id = 0)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var currentUserId = user.Id;

            CourseViewModel model = new CourseViewModel
            {
                CertificationImage = "Certification.jpg",  
                CertificationName = "قم بتحريك النص لتعديل وضعه بالشهادة", 
                CreatedBy = currentUserId,
                CreationDate = DateTime.Now,  
                CourseDate = DateTime.Now     
            };

      
            if (id != 0)
            {
                var existingCourse = await _dB.Courses.FindAsync(id);
                if (existingCourse != null)
                {
                    model = new CourseViewModel
                    {
                        CourseId = existingCourse.CourseId,
                        CourseName = existingCourse.CourseName,
                        CoachName = existingCourse.CoachName,
                        CertificationImage = existingCourse.CertificationImage,
                        CertificationName = existingCourse.CertificationName,
                        CreationDate = existingCourse.CreationDate,
                        CourseDate = existingCourse.CourseDate,
                        CreatedBy = existingCourse.CreatedBy
                    };
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCourse(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Handle image upload
            if (model.CertificationImageFile != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Attachments");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.CertificationImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CertificationImageFile.CopyToAsync(stream);
                }
                model.CertificationImage = fileName;
            }
            else if (string.IsNullOrEmpty(model.CertificationImage))
            {
                model.CertificationImage = "Certification.jpg";  // Default if no image is provided
            }
            if (model.CourseId == 0)
            {
                var newCourse = new Course
                {
                    CourseName = model.CourseName,
                    CoachName = model.CoachName,
                    CertificationImage = model.CertificationImage,
                    CertificationName = model.CertificationName,
                    CreationDate = DateTime.Now,
                    CourseDate = model.CourseDate,
                    CreatedBy = model.CreatedBy
                };
                _dB.Courses.Add(newCourse);
            }
            else
            {
                // Update existing course
                var existingCourse = await _dB.Courses.FindAsync(model.CourseId);
                if (existingCourse != null)
                {
                    existingCourse.CourseName = model.CourseName;
                    existingCourse.CoachName = model.CoachName;
                    existingCourse.CertificationImage = model.CertificationImage;
                    existingCourse.CertificationName = model.CertificationName;
                    existingCourse.CourseDate = model.CourseDate;
                    _dB.Entry(existingCourse).State = EntityState.Modified;
                }
            }
            await _dB.SaveChangesAsync();
            return RedirectToAction("Index", "Course");
        }



        #region ActiveCertificationsold

        //public IActionResult ActiveCertifications(int CourseId)
        //{
        //    var course = _dB.Courses.Find(CourseId); 
        //    var workshopParticipants = _dB.WorkshopParticipants
        //        .AsNoTracking()
        //        .Where(x => x.CourseId == CourseId && !x.IsEmailSended)
        //        .ToList();

        //    try
        //    {
        //        // Get the path to the CertificationEmail.html template
        //        var serverVal = Path.Combine(_webHostEnvironment.WebRootPath, "Views", "Shared",
        //            "CertificationEmail.html");

        //        // Queue the email sending process using Hangfire
        //        BackgroundJob.Enqueue(() => SendEmail(
        //            string.Concat("شهادة حضور ورشة عمل ", course.CourseName),
        //            workshopParticipants,
        //            serverVal,
        //            course.CourseName,
        //            course.CourseDate,
        //            course.CoachName)
        //        );
        //    }
        //    catch (Exception e)
        //    {
        //        return Json("-1"); 
        //    }

        //    return Json("1"); 

        //}
        #endregion


        public IActionResult ActiveCertifications(int CourseId)
        {
            var course = _dB.Courses.Find(CourseId);
            var workshopParticipants = _dB.WorkshopParticipants
                .AsNoTracking()
                .Where(x => x.CourseId == CourseId && !x.IsEmailSended)
                .ToList();

            try
            {
               
                var serverVal = Path.Combine(_webHostEnvironment.WebRootPath, "Views", "Shared", "CertificationEmail.html");

              
                _backgroundJobClient.Enqueue(() => SendEmail(
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
                return Json("-1");
            }

            return Json("1");
        }

    }
}