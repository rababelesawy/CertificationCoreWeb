

using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Certification.Domain.Entities;
using Certification.Infrastructure.Data;
using CertificationWeb.CustomAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CertificationWeb.Controllers
{
    [Authorize]
    // [CustomAuthorize]
    public class BaseController : Controller
    {
        private readonly Context _dB;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {

            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public BaseController(Context DB)
        {
            _dB = DB;
        }

 
        public CustomPrincipal CurrentUser
        {

            #region 
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user != null && user.Identity.IsAuthenticated)
                {
                    // Assuming CustomPrincipal is a constructor that takes ClaimsPrincipal
                    return new CustomPrincipal(user as ClaimsPrincipal);
                }
                return null;
            }

            #endregion



        }

        public static string GenerateuserCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            return randomNumber.ToString();
        }


        //public void UpdateCurrentUserData(User model)
        //{
        //    HttpCookie cookie = new HttpCookie("BaseAdminCookie", "");
        //    cookie.Expires = DateTime.Now.AddYears(-1);
        //    Response.Cookies.Add(cookie);
        //    var user = userService.ValidateUser(model.Email, model.Password);
        //    user = userService.GetUser(CurrentUser.UserId);
        //    CustomSerializeModel userModel = new CustomSerializeModel()
        //    {
        //        UserId = user.UserId,
        //        Name = user.Name,
        //        TypeId = user.TypeId.Value,
        //        Email = user.Email,
        //        ImageId = user.ImageId ?? 0,
        //        Roles = user.Roles.Select(s => s.Name).ToList()
        //    };

        //    string userData = JsonConvert.SerializeObject(userModel);
        //    FormsAuthenticationTicket authTicket =
        //        new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

        //    string enTicket = FormsAuthentication.Encrypt(authTicket);
        //    HttpCookie faCookie = new HttpCookie("BaseAdminCookie", enTicket);
        //    Response.Cookies.Add(faCookie);


        //}


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    int typeId = file.ContentType.Contains("pdf") ? 2 : 1; // 2 for pdf, 1 for image

                    var attachment = new FileAttachment
                    {
                        TypeId = typeId,
                        CreationDate = DateTime.Now,
                        Title = Path.GetFileName(file.FileName)
                    };

                    _dB.FileAttachment.Add(attachment);
                    await _dB.SaveChangesAsync();

                    string _FileName = Path.GetFileName(file.FileName);
                    string hypoName = "/Attachments/" + attachment.FileAttachmentId.ToString() + (typeId == 1 ? ".jpg" : ".pdf");
                    string _path1 = Path.Combine(_hostingEnvironment.WebRootPath, "Attachments",
                                                 attachment.FileAttachmentId.ToString() + (typeId == 1 ? ".jpg" : ".pdf"));

                    using (var stream = new FileStream(_path1, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Json(new
                    {
                        path = hypoName,
                        id = attachment.FileAttachmentId.ToString(),
                        name = _FileName,
                        date = DateTime.Now.ToString("yyyy-MM-dd"),
                    });
                }

                return Json(new { message = "Sorry, no file to upload." });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry, can't upload your file. " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        int typeId = file.ContentType.Contains("pdf") ? 2 : 1; // 2 for pdf, 1 for image

                        var attachment = new FileAttachment
                        {
                            TypeId = typeId,
                            CreationDate = DateTime.Now,
                            Title = Path.GetFileName(file.FileName)
                        };

                        _dB.FileAttachment.Add(attachment);
                        await _dB.SaveChangesAsync();

                        string _FileName = Path.GetFileName(file.FileName);
                        string hypoName = "/Attachments/" + attachment.FileAttachmentId.ToString() + (typeId == 1 ? ".jpg" : ".pdf");
                        string _path1 = Path.Combine(_hostingEnvironment.WebRootPath, "Attachments",
                                                     attachment.FileAttachmentId.ToString() + (typeId == 1 ? ".jpg" : ".pdf"));

                        using (var stream = new FileStream(_path1, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return Json(new
                        {
                            path = hypoName,
                            id = attachment.FileAttachmentId.ToString(),
                            name = _FileName,
                            date = DateTime.Now.ToString("yyyy-MM-dd"),
                        });
                    }
                }

                return Json(new { message = "Sorry, no files to upload." });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry, can't upload your files. " + ex.Message });
            }
        }

        public bool IsAjaxRequest()
        {
            return HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        [NonAction]
        public int SendHtmlFormattedEmail(string Subject, string Body, string ToEmail)
        {
            try
            {

                var fromEmail = new MailAddress("repoteq.test@gmail.com", "نظام إصدار الشهادات");

                var toEmail = new MailAddress(ToEmail);

                var fromEmailPassword = "rep123@456";
                string subject = Subject;

                string body = Body;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword),

                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })

                    smtp.Send(message);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }




        private string createEmailBody(string link, string ServerVal, string CourseName, DateTime CourseDate, string CoachName)
        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            try
            {
                using (StreamReader reader = new StreamReader(ServerVal))
                {
                    body = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {

            }
            //replacing the required things  
            body = body.Replace("{CourseName}", CourseName);
            body = body.Replace("{CourseDate}", CourseDate.ToString("MM/dd/yyyy"));
            body = body.Replace("{link}", link);
            body = body.Replace("{CoachName}", CoachName);

            return body;

        }




        public int SendEmail(string Subject, List<WorkshopParticipant> LIST, string ServerVal, string CourseName, DateTime CourseDate, string CoachName)
        {
            int i = 0;
            foreach (WorkshopParticipant item in LIST)
            {
                if (item.Email != null && !item.IsEmailSended)
                {
                    try
                    {
                        string url = string.Format("https://cert.repoteq.com/Home/EditUserInfo?Id={0}", item.WorkshopParticipantId);
                        string body = this.createEmailBody(url, ServerVal, CourseName, CourseDate, CoachName);

                        var res = this.SendHtmlFormattedEmail(Subject, body, item.Email);
                        if (res == 1)
                        {
                            item.IsEmailSended = true;
                            item.Course = null;

                            // Use Update method instead of setting EntityState
                            _dB.Update(item);
                            _dB.SaveChangesAsync(); // Make sure to await for async operation
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle exception (optional logging)
                    }
                }

                i++;
                if (i % 80 == 0)
                    Thread.Sleep(TimeSpan.FromSeconds(120));
            }
            return 1;
        }



        //public string ConvertViewToString(string viewName, object model, ControllerContext context)
        //{
        //    ViewData.Model = model;

        //    using (StringWriter writer = new StringWriter())
        //    {
        //        try
        //        {
        //            ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
        //            ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
        //            vResult.View.Render(vContext, writer);
        //            return writer.ToString();
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        return "";
        //    }
        //}

    }
}
    