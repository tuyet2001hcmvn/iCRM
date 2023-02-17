using ISD.API.EntityModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.API.Repositories
{
    public interface ITemplateAndGiftCampaignRepository
    {
        IQueryable<TemplateAndGiftCampaignModel> Search(int? templateAndGiftCampaignCode, string templateAndGiftCampaignName, out int totalRow);
    }
    public class TemplateAndGiftCampaignRepository : GenericRepository<TemplateAndGiftCampaignModel>, ITemplateAndGiftCampaignRepository
    {
        public TemplateAndGiftCampaignRepository(ICRMDbContext context) : base(context)
        {
        }
        public IQueryable<TemplateAndGiftCampaignModel> Search(int? templateAndGiftCampaignCode, string templateAndGiftCampaignName, out int totalRow)
        {
            totalRow = 0;
            var list = from campaign in context.TemplateAndGiftCampaignModels.Include(s => s.CreateByNavigation).Include(s => s.LastEditByNavigation)
                       where (templateAndGiftCampaignCode == null || campaign.TemplateAndGiftCampaignCode == templateAndGiftCampaignCode)
                       && (templateAndGiftCampaignName == null || templateAndGiftCampaignName == "" || campaign.TemplateAndGiftCampaignName.Contains(templateAndGiftCampaignName))              
                       orderby (campaign.TemplateAndGiftCampaignCode) descending
                       select campaign;
            totalRow = list.Count();
            return list;
        }
    }
}
