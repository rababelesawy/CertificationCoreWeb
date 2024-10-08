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
using System.Drawing.Printing;
using Microsoft.AspNetCore.Identity;


namespace CertificationWeb.Controllers
{

    //[Authorize]
    public class HomeController :BaseController
    {
        private readonly Context _dB;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(Context DB , IHttpContextAccessor httpContextAccessor, UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager) :base(DB)
        {
            _dB = DB;
            _httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        #region Old Index
        //public IActionResult Index()
        //{
        //    var courses = _dB.Courses.ToList();

        //    // Create a list of SelectListItems
        //    ViewBag.Courses = courses.Select(s =>
        //        new SelectListItem
        //        {
        //            Text = s.CourseName,
        //            Value = s.CourseId.ToString()
        //        }).ToList();

        //    // Initialize the WorkshopParticipantModel
        //    var model = new WorkshopParticipantModel
        //    {
        //        WorkshopParticipantSearchModel = new WorkshopParticipantSearchModel() // Ensure this is initialized
        //    };

        //    return View(model);

        //}

        #endregion


        #region New index
        public IActionResult Index(int? page)
        {
          
            var courses = _dB.Courses.ToList();

    
            ViewBag.Courses = courses.Select(s => new SelectListItem
            {
                Text = s.CourseName,
                Value = s.CourseId.ToString()
            }).ToList();

          
            var searchModel = new WorkshopParticipantSearchModel();

         
            var participantsQuery = _dB.WorkshopParticipants.AsQueryable();

            // Pagination logic
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var paginatedParticipants = participantsQuery
                .OrderBy(p => p.WorkshopParticipantId) 
                .ToPagedList(pageNumber, pageSize);

        
            var model = new WorkshopParticipantModel
            {
                TotalUser = participantsQuery.Count(),
                PrintUser = participantsQuery.Count(p => p.IsPrinted == true),
                NonPrintUser = participantsQuery.Count(p => !p.IsPrinted == true),
                WorkshopParticipantSearchModel = searchModel,
                WorkshopParticipants = new PagedResultViewModel<WorkshopParticipant>
                {
                    Items = paginatedParticipants, 
                    TotalCount = paginatedParticipants.TotalItemCount,
                    CurrentPage = paginatedParticipants.PageNumber,
                    ItemsPerPage = paginatedParticipants.PageSize
                }
            };

            return View(model);
        }
        #endregion





        #region  old workshopparticipation
        //public IActionResult WorkshopParticipants(int itemsPerPage = 10, int currentPage = 1)
        //{
        //    ViewBag.Theme = "custom";
        //    ViewBag.Type = 1;
        //    ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

        //    // Get current user id
        //    var currentUserId = CurrentUser?.UserId;
        //    if (currentUserId == null)
        //    {

        //        return Unauthorized(); // 401 Unauthorized instead of 404
        //    }

        //    // Get workshop participants
        //    var workshopParticipants = _dB.WorkshopParticipants
        //        .Where(x => x.Course.CreatedBy == currentUserId)
        //        .ToList();

        //    var totalUserCount = workshopParticipants.Count();
        //    var printUserCount = workshopParticipants.Count(x => x.IsPrinted == true);
        //    var nonPrintUserCount = workshopParticipants.Count(x => x.IsPrinted != true);

        //    var pagedWorkshopParticipants = workshopParticipants
        //        .OrderByDescending(x => x.CourseId)
        //        .Skip((currentPage - 1) * itemsPerPage)
        //        .Take(itemsPerPage)
        //        .ToList();

        //    var result = new WorkshopParticipantModel
        //    {
        //        WorkshopParticipantSearchModel = new WorkshopParticipantSearchModel(),
        //        WorkshopParticipants = new PagedResultViewModel<WorkshopParticipant>
        //        {
        //            Items = (IPagedList<WorkshopParticipant>)pagedWorkshopParticipants,
        //            TotalCount = totalUserCount,
        //            CurrentPage = currentPage,
        //            ItemsPerPage = itemsPerPage
        //        },
        //        TotalUser = totalUserCount,
        //        PrintUser = printUserCount,
        //        NonPrintUser = nonPrintUserCount
        //    };

        //    return PartialView("_WorkshopParticipantList", result);
        //}




        #endregion

      

        public async Task<IActionResult> WorkshopParticipants(int itemsPerPage = 10, int currentPage = 1)
        {
            ViewBag.Theme = "custom";
            ViewBag.Type = 2;
            ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

          
            var workshopParticipants = await _dB.WorkshopParticipants
                .OrderByDescending(x => x.CourseId) 
                .ToListAsync();

            var totalUserCount = workshopParticipants.Count();
            var printUserCount = workshopParticipants.Count(x => x.IsPrinted == true);
            var nonPrintUserCount = workshopParticipants.Count(x => !x.IsPrinted == true);

            // Paginate workshop participants
            var pagedWorkshopParticipants = workshopParticipants
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            var pagedList = pagedWorkshopParticipants.ToPagedList(currentPage, itemsPerPage, totalUserCount);

          
            var result = new WorkshopParticipantModel
            {
                WorkshopParticipantSearchModel = new WorkshopParticipantSearchModel(),
                WorkshopParticipants = new PagedResultViewModel<WorkshopParticipant>
                {
                    Items = pagedList, 
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




        #region  old WorkshopParticipantSearch
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

        #endregion


        #region New WorkshopParticipantSearch
        [HttpPost]
        public async Task<IActionResult> WorkshopParticipantSearch(WorkshopParticipantSearchModel WorkshopParticipantSearchModel, int itemsPerPage = 10, int currentPage = 1)
        {
            ViewBag.Theme = "custom";
            ViewBag.Index = itemsPerPage * (currentPage - 1) + 1;

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var currentUserId = user.Id;

      
            var workshopParticipantsQuery = _dB.WorkshopParticipants
                .Include(x => x.Course) 
                .Where(x => x.Course.CreatedBy == currentUserId);

            if (WorkshopParticipantSearchModel != null)
            {
                if (!string.IsNullOrEmpty(WorkshopParticipantSearchModel.Name))
                {
                    workshopParticipantsQuery = workshopParticipantsQuery.Where(x => x.Name.Contains(WorkshopParticipantSearchModel.Name));
                }

                if (!string.IsNullOrEmpty(WorkshopParticipantSearchModel.Phone))
                {
                    workshopParticipantsQuery = workshopParticipantsQuery.Where(x => x.Phone == WorkshopParticipantSearchModel.Phone);
                }

                if (WorkshopParticipantSearchModel.CourseId != 0)
                {
                    workshopParticipantsQuery = workshopParticipantsQuery.Where(x => x.CourseId == WorkshopParticipantSearchModel.CourseId);
                }

                if (!string.IsNullOrEmpty(WorkshopParticipantSearchModel.CoachName))
                {
                    workshopParticipantsQuery = workshopParticipantsQuery.Where(x => x.Course.CoachName.Contains(WorkshopParticipantSearchModel.CoachName));
                }

                if (WorkshopParticipantSearchModel.IsPrinted != null)
                {
                    workshopParticipantsQuery = workshopParticipantsQuery.Where(x => x.IsPrinted == WorkshopParticipantSearchModel.IsPrinted);
                }

                if (WorkshopParticipantSearchModel.IsSended != null)
                {
                    workshopParticipantsQuery = workshopParticipantsQuery.Where(x => x.IsEmailSended == WorkshopParticipantSearchModel.IsSended);
                }
            }

      
            var totalUserCount = await workshopParticipantsQuery.CountAsync();

        
            var printUserCount = await workshopParticipantsQuery.CountAsync(x => x.IsPrinted == true);
            var nonPrintUserCount = await workshopParticipantsQuery.CountAsync(x => x.IsPrinted != true);

        
            var pagedWorkshopParticipants = await workshopParticipantsQuery
                .OrderByDescending(x => x.CourseId)
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var result = new WorkshopParticipantModel
            {
                WorkshopParticipantSearchModel = WorkshopParticipantSearchModel ?? new WorkshopParticipantSearchModel(),
                WorkshopParticipants = new PagedResultViewModel<WorkshopParticipant>
                {
                    Items = pagedWorkshopParticipants.Select(x => new WorkshopParticipant
                    {
                        Name = x.Name,
                        Phone = x.Phone,
                        IsPrinted = x.IsPrinted,
                        IsEmailSended = x.IsEmailSended,
                        Course = x.Course // Ensure Course is included
                    }).ToPagedList(currentPage, itemsPerPage),
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














        #endregion



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
        public ActionResult UserInfo(WorkshopParticipant model ) // how it implement
        {
            var WorkshopParticipant = _dB.WorkshopParticipants.Find(model.WorkshopParticipantId);
            WorkshopParticipant.Name = model.Name;
            WorkshopParticipant.Phone = model.Phone;
            WorkshopParticipant.Email = model.Email;
            _dB.SaveChanges();
            return Json("1");
        }

        #region  Old GetCertification
        //[AllowAnonymous]
        //public ActionResult GetCertification(Guid Id)
        //{
        //    //CourseParticipationModel model = new CourseParticipationModel { workshopParticipant= };
        //    //db.WorkshopParticipants.Where(x => x.WorkshopParticipantId == Id).FirstOrDefault();

        //    return View("Certification", _dB.WorkshopParticipants.Where(x => x.WorkshopParticipantId == Id).FirstOrDefault());
        //}
        #endregion


        #region New GetCertification

        [AllowAnonymous]
        public ActionResult GetCertification(Guid Id)
        {
           
            var participant = _dB.WorkshopParticipants.FirstOrDefault(x => x.WorkshopParticipantId == Id);

          
            if (participant == null)
            {
                return NotFound(); 
            }

            
            if (participant.CourseId == null)
            {
                
                ModelState.AddModelError(string.Empty, "Course ID is not set for this participant.");
                return View("_Certification", participant);
            }

           
            var course = _dB.Courses.FirstOrDefault(c => c.CourseId == participant.CourseId);

            CourseParticipationModel model = new CourseParticipationModel
            {
                workshopParticipant = participant,
                course = course
            };


            return PartialView("_Certification" , model);
        }
        #endregion





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
        public ActionResult Certification(WorkshopParticipant model)   //// how it implement
        {
            var model2 = _dB.WorkshopParticipants
                .Include(wp => wp.Course) 
                .FirstOrDefault(x => x.WorkshopParticipantId == model.WorkshopParticipantId);

            if (model2 == null)
            {
               
                return NotFound(); 
            }

            return PartialView("_Certification", model2);
        }


    }
}