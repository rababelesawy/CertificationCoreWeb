using Certification.Domain.Entities;

namespace Certification.Domain.DomainModels
{
    public class CourseParticipationModel
    {
        public WorkshopParticipant workshopParticipant { get; set; }
        public Course course { get; set; }
    }
}
