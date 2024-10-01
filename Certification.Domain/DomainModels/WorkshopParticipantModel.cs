

using Certification.Domain.Entities;

namespace Certification.Domain.DomainModels
{
    public class WorkshopParticipantModel
    {


        public int TotalUser { get; set; }
        public int PrintUser { get; set; }
        public int NonPrintUser { get; set; }
        public WorkshopParticipantSearchModel WorkshopParticipantSearchModel { get; set; }
        public Ifa.Model.PagedResultViewModel<WorkshopParticipant> WorkshopParticipants { get; set; }
    }


    public class WorkshopParticipantSearchModel
    {

        public string Name { get; set; }

        public string Phone { get; set; }
        public int CourseId { get; set; }
        public bool? IsPrinted { get; set; }
        public bool? IsSended { get; set; }
        public string CoachName { get; set; }
    }
}

