using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certification.Domain.Entities
{
    public class FileAttachment
    {
        public int FileAttachmentId { get; set; }
        public int? TypeId { get; set; }
        public string Title { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
