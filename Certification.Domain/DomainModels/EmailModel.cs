namespace Certification.Domain.DomainModels
{
    public class EmailModel
    {
        public string UserName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Dearcustomer { get; set; }
        public string Footermessage { get; set; }

        public bool IsAccountConfirmation { get; set; }
        public string HomeLink { get; set; }
        public string AboutLink { get; set; }
        public string ContactLink { get; set; }
    }
}
