using System;

namespace ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels
{
    public class CheckInViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
}
