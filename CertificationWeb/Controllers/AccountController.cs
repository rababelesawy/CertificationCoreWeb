using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Certification.Infrastructure.Data;
using Certification.Domain.DomainModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CertificationWeb.Controllers;
using CertificationWeb.CustomAuthentication;
using Newtonsoft.Json;


namespace CertificationWebeWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly Context _db;
        private readonly UserManager<CustomMembershipUser> _userManager;
        private readonly SignInManager<CustomMembershipUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(Context Db, UserManager<CustomMembershipUser> userManager,
            SignInManager<CustomMembershipUser> signInManager, RoleManager<IdentityRole> roleManager) : base(Db)
        {
            _db = Db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Login(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        authProperties);
                    return new JsonResult("1");
                }
                else
                {
                    return new JsonResult("-1");
                }
            }

            return new JsonResult("-1");
        }


        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgetPassword(string Email)
        {
            //if (Email != null && Email != "")
            //{
            //    var customer = userService.GetUser(Email);
            //    if (customer == null)
            //        return Json("-1");


            //    var url = string.Format("/Account/ResetPassword?id={0}", customer.UserId);
            //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            //    string subject = "Forget Password !";
            //    string body = "<br/> Please click on the following link to reset Password" + "<br/><a href='" + link + "'> Reset Password! </a>";
            //    int res = SendEmail("Reset Password",new EmailModel { Body = body, ToEmail = Email, Subject = subject ,UserName=customer.Name});
            //    return Json(res.ToString());
            //}
            //else
            //{
            //    return Json("-1");
            //}

            return null;
        }


        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            var user = await _userManager.FindByIdAsync(CurrentUser.UserId.ToString());

            if (user == null)
            {
                return NotFound();
            }

            return PartialView("_ResetPassword", new ResetPasswordView
            {
                Email = user.Email,
                Id = user.UserId, // Ensure this is the correct property from your User class
                UserName = user.UserName
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordView resetPasswordView)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordView.Id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            // Reset password
            var resetPasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!resetPasswordResult.Succeeded)
            {
                return Json("0"); // Indicate failure
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, resetPasswordView.Password);
            if (!addPasswordResult.Succeeded)
            {
                return Json("0"); // Indicate failure
            }

            // Serialize user data for claims
            var userModel = new CustomSerializeModel
            {
                UserId = user.UserId,
                Name = user.FullName,
                UserName = user.Email
            };

            string userData = JsonConvert.SerializeObject(userModel);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserData", userData)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Json("1"); // Indicate success
        }


        public ActionResult Register()
        {
            // ViewBag.ListCategories = categoryService.GetSelectListCtegories();
            // ViewBag.ListCountries = countryService.GetSelectListCountries();
            // ViewBag.ListCities = cityService.GetSelectListCities(0);

            return View(new RegistrationView { ImageId = 0, TypeId = 1 });
        }


        [HttpPost]
        public ActionResult Register(RegistrationView model)
        {
            //if (ModelState.IsValid)
            //{
            //    // Email Verification
            //    if (userService.GetUser(model.Email) != null)
            //    {
            //        ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
            //        return View(model);
            //    }

            //    //Save User Data 
            //    var user = Mapper.Map<RegistrationView, User>(model);
            //    user.Address = "";
            //    user.IsActive = true;
            //    user.Code = user.TypeId == 2 ? "OneQouteSupp" + GenerateuserCode() : "OneQouteClient" + GenerateuserCode();
            //    userService.CreateUser(user);

            //    user.Roles.Add(roleService.GetRole(model.TypeId));

            //    userService.SaveUser();


            //    //Verification Email
            //    var code = RandomString(4);
            //    user.ActivationCode = code;
            //    userService.UpdateUser(user);
            //    userService.SaveUser();
            //    if (IsAjaxReuqest()) { return Json(new { code = "1", activationcode = code,id=user.UserId }); }


            //    //send email
            //    VerificationEmail(model.Email, code, user.UserId, model.Name);

            //    return View("RegisterSuccess");

            //}

            //ViewBag.ListCategories = categoryService.GetSelectListCtegories();
            //ViewBag.ListCountries = countryService.GetSelectListCountries();
            //ViewBag.ListCities = cityService.GetSelectListCities(0);
            //return View(model);


            return null;
        }


        [HttpGet]
        public ActionResult ActivationAccount(string ActivationCode, int id)
        {
            //bool statusAccount = false;

            //var user = userService.GetUserActiviationCode(id,ActivationCode);

            //if (user != null)
            //{
            //    user.IsActive = true;
            //    user.IsEmailVerified = true;
            //    userService.SaveUser();
            //    statusAccount = true;

            //    user.CreationDate = DateTime.Now;
            //    var res = Request.Cookies["UniCertificationCookie"];
            //    CustomSerializeModel userModel = new CustomSerializeModel()
            //    {
            //        UserId = user.UserId,
            //        Name = user.Name,
            //        TypeId = user.TypeId.Value,
            //        Email = user.Email,
            //        ImageId = user.ImageId ?? 0,
            //        CityId = user.CityId ?? 0,
            //        CountryId = user.CountryId ?? 0,
            //        CategoryId = user.CategoryId ?? 0,
            //        Roles = user.Roles.Select(s => s.Name).ToList()
            //    };


            //    string userData = JsonConvert.SerializeObject(userModel);
            //    FormsAuthenticationTicket authTicket =
            //        new FormsAuthenticationTicket(1, userModel.Email, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

            //    string enTicket = FormsAuthentication.Encrypt(authTicket);
            //    HttpCookie faCookie = new HttpCookie("UniCertificationCookie", enTicket);
            //    Response.Cookies.Add(faCookie);

            //}
            //else
            //{
            //    ViewBag.Message = "Something Wrong !!";
            //}

            //if (IsAjaxReuqest()) { return Json("1",JsonRequestBehavior.AllowGet); }
            //ViewBag.Status = statusAccount;
            //return RedirectToAction("Index", "Home");


            return null;
        }


        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();


            Response.Cookies.Delete("UniCertificationCookie");


            return RedirectToAction("Login", "Account");
        }


        //[NonAction]
        //public void VerificationEmail(string email, string activationCode, int id, string username)
        //{
        //    var url = string.Format("/Account/ActivationAccount?activationCode={0}&id={1}", activationCode, id);
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

        //    string subject = "Activation Account !";
        //    string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

        //    SendEmail("Activation Account",new EmailModel { Body = body, ToEmail = email, Subject = subject, UserName = username, IsAccountConfirmation = true });
        //}


        public ActionResult profile()
        {
            return View();
        }

        //public ActionResult ChangePhoto()
        //{
        //    var user = db.Users.Find(CurrentUser.UserId);

        //    return PartialView("_ChangePhoto", new ChangeImageView() { Id = user.UserId, ImageId = user.ImageId });
        //}

        //[HttpPost]
        //public ActionResult ChangePhoto(ChangeImageView model)
        //{
        //    var user = db.Users.Find(model.Id);
        //    user.ImageId = model.ImageId;
        //    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    return Json("1");
        //}
    }
}