using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certification.Domain.Entities
{
    public class WorkshopParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkshopParticipantId { get; set; }

        //public Guid GuidId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        //public DateTime AddedDate { get; set; }
        public Nullable<bool> IsPrinted { get; set; }
        public bool IsEmailSended { get; set; }
        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}