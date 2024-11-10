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
using Certification.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CertificationWeb.Controllers;
using Newtonsoft.Json;


namespace CertificationWebeWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly Context _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(Context Db, UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager) : base(Db)
        {
            _db = Db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }




        #region  Login old
        //[HttpGet]
        //public IActionResult Login(string ReturnUrl = "")
        //{
        //    ViewBag.ReturnUrl = ReturnUrl;
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginView model, string returnUrl = "")
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var user = await _userManager.FindByNameAsync(model.UserName);
        //            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //            {
        //                var claims = new List<Claim>
        //                {
        //                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //                    new Claim(ClaimTypes.Name, user.UserName),
        //                    new Claim(ClaimTypes.Email, user.Email)
        //                };

        //                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //                var principal = new ClaimsPrincipal(identity);

        //                var authProperties = new AuthenticationProperties
        //                {
        //                    IsPersistent = false,
        //                    ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
        //                };

        //                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
        //                    authProperties);
        //                return new JsonResult("1");
        //            }
        //            else
        //            {
        //                return new JsonResult("-1");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }

        //    var errors = ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).ToList();
        //    return new JsonResult("-1");
        //}


        #endregion




        #region  New Login 

           [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
           ViewBag.ReturnUrl = returnUrl;
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView model , string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
             
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                  
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        // Create claims for the user
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                      
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                      
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                     
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
                        };

                       
                        await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        ////return RedirectToAction("Index", "Home");
                        return new JsonResult("1");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return new JsonResult("-1");
            }

            return new JsonResult("-1");
        }

        #endregion





        #region Old Register
        //[HttpGet]
        //public ActionResult Register()
        //{
        //    // ViewBag.ListCategories = categoryService.GetSelectListCtegories();
        //    // ViewBag.ListCountries = countryService.GetSelectListCountries();
        //    // ViewBag.ListCities = cityService.GetSelectListCities(0);

        //    return View(new RegistrationView { ImageId = 0, TypeId = 1 });
        //}






        //[HttpPost]
        //public ActionResult Register(RegistrationView model)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    // Email Verification
        //    //    if (userService.GetUser(model.Email) != null)
        //    //    {
        //    //        ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
        //    //        return View(model);
        //    //    }

        //    //    //Save User Data 
        //    //    var user = Mapper.Map<RegistrationView, User>(model);
        //    //    user.Address = "";
        //    //    user.IsActive = true;
        //    //    user.Code = user.TypeId == 2 ? "OneQouteSupp" + GenerateuserCode() : "OneQouteClient" + GenerateuserCode();
        //    //    userService.CreateUser(user);

        //    //    user.Roles.Add(roleService.GetRole(model.TypeId));

        //    //    userService.SaveUser();


        //    //    //Verification Email
        //    //    var code = RandomString(4);
        //    //    user.ActivationCode = code;
        //    //    userService.UpdateUser(user);
        //    //    userService.SaveUser();
        //    //    if (IsAjaxReuqest()) { return Json(new { code = "1", activationcode = code,id=user.UserId }); }


        //    //    //send email
        //    //    VerificationEmail(model.Email, code, user.UserId, model.Name);

        //    //    return View("RegisterSuccess");

        //    //}

        //    //ViewBag.ListCategories = categoryService.GetSelectListCtegories();
        //    //ViewBag.ListCountries = countryService.GetSelectListCountries();
        //    //ViewBag.ListCities = cityService.GetSelectListCities(0);
        //    //return View(model);


        //    return null;
        //}


        #endregion




        #region Register New
        //[HttpGet]
        //public IActionResult Register()
        //{
        //    var model = new RegistrationView();
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegistrationView model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        //        if (existingUser != null)
        //        {
        //            ModelState.AddModelError("Email", "Warning: This email already exists.");
        //            return View(model);
        //        }
        //        var phoneNumber = string.IsNullOrEmpty(model.Phone) ? "N/A" : model.Phone;
        //        var user = new User
        //        {
        //            UserName = model.Email.Split('@')[0],
        //            Email = model.Email,
        //            PhoneNumber = model.Phone, // This maps to the IdentityUser's PhoneNumber property
        //            Phone = model.Phone, // This maps to the custom Phone property in your User entity               
        //            NameAr = model.Name,
        //            Address = model.Address,
        //        };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            var claims = new List<Claim>
        //          {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(ClaimTypes.Email, user.Email)
        //          };
        //            string roleName = model.TypeId == 1 ? "Client" : "Supplier";


        //            if (!await _roleManager.RoleExistsAsync(roleName))
        //            {
        //                await _roleManager.CreateAsync(new IdentityRole(roleName));
        //            }
        //            await _userManager.AddToRoleAsync(user, roleName);
        //            claims.Add(new Claim(ClaimTypes.Role, roleName));

        //            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //            var authProperties = new AuthenticationProperties
        //            {
        //                IsPersistent = false,
        //                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
        //            };
        //            await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);


        //            return RedirectToAction("Index", "Home");
        //        }

        //        // Add errors to ModelState to display in the view
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    return View(model);
        //}
        #endregion


        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegistrationView();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegistrationView model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Warning: This email already exists.");
                    return View(model);
                }

                var phoneNumber = string.IsNullOrEmpty(model.Phone) ? "N/A" : model.Phone;
                var user = new User
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    PhoneNumber = model.Phone, // IdentityUser's PhoneNumber
                    Phone = model.Phone, // Custom Phone property
                    NameAr = model.Name,
                    Address = model.Address,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

                    string roleName;
                    if (model.TypeId == 1)
                    {
                        roleName = "Client";
                    }
                    else if (model.TypeId == 2)
                    {
                        roleName = "Supplier";
                    }
                    else if (model.TypeId == 3)
                    {
                        roleName = "FreeUser"; // Add FreeUser role for TypeId == 3
                    }
                    else
                    {
                        // Handle other cases
                        roleName = "Client"; // Default role
                    }

                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                    await _userManager.AddToRoleAsync(user, roleName);
                    claims.Add(new Claim(ClaimTypes.Role, roleName));

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
                    };
                    await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);

                    return RedirectToAction("Index", "Home");
                }

                // Add errors to ModelState to display in the view
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }





        #region
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

        #region ForgetPassword Asp.netcore
        //[HttpPost]
        //public async Task<JsonResult> ForgetPassword(string email)
        //{
        //    if (string.IsNullOrWhiteSpace(email))
        //    {
        //        return Json("-1"); // Email is empty
        //    }

        //    // Get the user by email
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return Json("-1"); // User not found
        //    }

        //    // Generate a password reset token
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, Request.Scheme);

        //    string subject = "Reset Password";
        //    string body = $"<br/> Please click on the following link to reset your password: <br/><a href='{url}'>Reset Password!</a>";

        //    // Call your email sending method here
        //    int res = SendEmail(subject, new EmailModel { Body = body, ToEmail = email, Subject = subject, UserName = user.UserName });

        //    return Json(res.ToString());
        //}


        #endregion






        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return PartialView("_ResetPassword", new ResetPasswordView
            {
                Email = user.Email,
                Id = user.Id, // Ensure this is the correct property from your User class
                UserName = user.UserName
            });
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordView resetPasswordView)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(); // Return NotFound if user is not found
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
                UserId = user.Id,
                Name = user.UserName,
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

        #endregion









        #region 
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


       
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

           
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



        ////public async Task<IActionResult> ChangePhoto()
        ////{
          
        ////    var user = await _userManager.GetUserAsync(User);

         
        ////    if (user == null)
        ////    {
        ////        return NotFound();
        ////    }

          
        ////    return PartialView("_ChangePhoto", new ChangeImageView { Id = user.Id, ImageId = user.ImageId });
        ////}


        ////[HttpPost]
        ////public async Task<IActionResult> ChangePhoto(ChangeImageView model)
        ////{
           
        ////    var user = await _userManager.GetUserAsync(User);

           
        ////    if (user == null)
        ////    {
        ////        return NotFound(); 
        ////    }

          
        ////    user.ImageId = model.ImageId;

       
        ////    var result = await _userManager.UpdateAsync(user);

          
        ////    if (!result.Succeeded)
        ////    {
              
        ////        return BadRequest(result.Errors);
        ////    }

        ////    return new JsonResult("1");
        ////}



        #endregion
    }
}