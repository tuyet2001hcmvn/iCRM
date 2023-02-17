using System;

namespace ISD.ViewModels.Marketing
{
    public class CheckInViewModel
    {
        public Guid? Id { get; set; }
        public Guid? CampaignId { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }

        public Guid? ProfileId { get; set; }
        public string Param { get; set; }
        //Số người tham gia
        public string NumberOfParticipant { get; set; }
    }
}
