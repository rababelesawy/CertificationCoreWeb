﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Certification.Domain.Entities
{
    public class User
    {
        public User()
        {

        }
        public int UserId { get; set; }

        public string NameAr { get; set; }

        public int ImageId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsEmailVerified { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}