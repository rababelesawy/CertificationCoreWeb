
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Certification.Domain.DomainModels
{
    public class LoginView
    {

        [Required(ErrorMessage = "اسم المستخدم غير صحيح ")]

        public string UserName { get; set; }


        [Required(ErrorMessage = " ")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "علي الاقل 6 احرف او ارقام")]
        public string Password { get; set; }

        public string? Token { get; set; }
        public bool RememberMe { get; set; }


    }

    public class CustomSerializeModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        //public int TypeId { get; set; }
        public string UserName { get; set; }
        //public int ImageId { get; set; }
        //public int CityId { get; set; }
        //public int CountryId { get; set; }
        //public int CategoryId { get; set; }

        public List<string> Roles { get; set; }

    }

    public class RegistrationView
    {

        [Required(ErrorMessage = "Name Is Required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Email Address Is Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone Is Required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        //[Required(ErrorMessage = "Address Is Required")]
        //[Display(Name = "Address")]
        public string Address { get; set; }


        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        public int TypeId { get; set; }
        public bool IsSocialRegisteration { get; set; }
        public int? CategoryId { get; set; }


        //[Required(ErrorMessage = "Commercial Register Is Required")]

        public string? CommercialRegister { get; set; }

        [Display(Name = "Logo")]
        public int ImageId { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Error : Confirm password does not match with password")]
        public string ConfirmPassword { get; set; }

    }

    public class UserView
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "*")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "*")]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "*")]
        [Remote("IsUserNameExist", "UserSystem", AdditionalFields = "UserId",
                ErrorMessage = "UserName name already exists")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress]
        [Remote("IsEmailExist", "UserSystem", AdditionalFields = "UserId",
                 ErrorMessage = "Email name already exists")]
        public string Email { get; set; }

        public bool IsActive { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Error : Confirm password does not match with password")]
        public string ConfirmPassword { get; set; }

    }

    public class ForgetPasswordView
    {

        [Required(ErrorMessage = "Email Address Is Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string Email { get; set; }

    }

    public class ResetPasswordView
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        [DataType(DataType.EmailAddress, ErrorMessage = "البريد الالكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "علي الاقل 6 احرف او ارقام")]
        public string Password { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "علي الاقل 6 احرف او ارقام")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "كلمة المرور و تاكيد كلمة المرور غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangeImageView
    {
        public string Id { get; set; }
        //[DataType("Image")]
        public int ImageId { get; set; }
    }
}
   