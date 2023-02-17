using ISD.API.ViewModels.MarketingViewModels.EmailAccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories.Services.Marketing
{
    public interface IEmailService
    {
        void TrackingOpened(Guid sendMailCalendarId);
        void Unsubscribe(Guid sendMailCalendarId);
    }
}
