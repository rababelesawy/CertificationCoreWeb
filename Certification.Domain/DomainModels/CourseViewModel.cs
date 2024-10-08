using Certification.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certification.Domain.DomainModels
{
    public class CourseViewModel
    { 
      public int CourseId { get; set; }

    [Required]
    public string CourseName { get; set; }

    [Required]
    public string CoachName { get; set; }


    public string CertificationImage { get; set; }




    public IFormFile CertificationImageFile { get; set; }

    public string CertificationName { get; set; }


    public DateTime CreationDate { get; set; }




    [Required]
    [DataType(DataType.Date), DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CourseDate { get; set; }

    [ForeignKey("User")]
    public string? CreatedBy { get; set; }

    public virtual User? User { get; set; }
}
}
