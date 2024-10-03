using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Certification.Domain.Entities
{
    public class User:IdentityUser
    {
        public User()
        {
        }
        public string NameAr { get; set; }

        public int ImageId { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}