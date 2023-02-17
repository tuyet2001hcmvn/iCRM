using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISD.API.Repositories
{
    public interface ITemplateAndGiftMemberRepository
    {
        IQueryable<ProfileModel> GetMember(Guid id, out int totalRow);
        IQueryable<ProfileModel> GetMemberByCampaign(Guid campaignId, string profileCode, string profileName, out int totalRow);
        TemplateAndGiftMemberViewModel GetMember(Guid campaignId, Guid profileId);
    }
    public class TemplateAndGiftMemberRepository : GenericRepository<TemplateAndGiftMemberModel>, ITemplateAndGiftMemberRepository
    {
        public TemplateAndGiftMemberRepository(ICRMDbContext context) : base(context)
        {
        }

        public IQueryable<ProfileModel> GetMember(Guid id, out int totalRow)
        {
            totalRow = 0;
            var listInfo = from member in context.TemplateAndGiftMemberModels
                           join profile in context.ProfileModels on member.ProfileId equals profile.ProfileId
                           where (member.TemplateAndGiftTargetGroupId == id)
                           orderby (profile.ProfileName) descending
                           select profile;
            totalRow = listInfo.Count();
            return listInfo;

        }
        public IQueryable<ProfileModel> GetMemberByCampaign(Guid campaignId, string profileCode, string profileName, out int totalRow)
        {
            totalRow = 0;
            var listInfo = from member in context.TemplateAndGiftMemberModels
                           join profile in context.ProfileModels on member.ProfileId equals profile.ProfileId
                           join campaign in context.TemplateAndGiftCampaignModels on member.TemplateAndGiftTargetGroupId equals campaign.TemplateAndGiftTargetGroupId
                           where (campaign.Id == campaignId)
                            && (profileCode == null || profileCode == "" || profile.ProfileCode.ToString().Contains(profileCode))
                           && (profileName == null || profileName == "" || profile.ProfileName.Contains(profileName))
                           orderby (profile.ProfileName) descending
                           select profile;
            var s = listInfo.ToList();
            totalRow = listInfo.Count();
            return listInfo;

        }
        public TemplateAndGiftMemberViewModel GetMember(Guid campaignId, Guid profileId)
        {
            
            var memberDetail = (from member in context.TemplateAndGiftMemberModels
                           join profile in context.ProfileModels on member.ProfileId equals profile.ProfileId
                           join campaign in context.TemplateAndGiftCampaignModels on member.TemplateAndGiftTargetGroupId equals campaign.TemplateAndGiftTargetGroupId                         
                           where (campaign.Id == campaignId)
                           && (profile.ProfileId == profileId)                         
                           orderby (profile.ProfileName) descending
                           select new TemplateAndGiftMemberViewModel { 
                                Id = member.Id,
                                ProfileName = profile.ProfileName,
                                Address = profile .Address
                           }).FirstOrDefault();
            var listAddress = context.TemplateAndGiftMemberAddressModels.Include
                (s=>s.Product).Where(s => s.TempalteAndGiftMemberId == memberDetail.Id);
            memberDetail.ListDetail = new List<TemplateAndGiftMemberAddressViewModel>();
            foreach (var item in listAddress)
            {
                TemplateAndGiftMemberAddressViewModel address = new TemplateAndGiftMemberAddressViewModel();
                address.TempalteAndGiftMemberId = memberDetail.Id;
                address.Address = item.Address;
                address.ProductName = item.Product.ProductName;
                address.ProductId = item.ProductId;
                address.Quantity = item.Quantity;
                address.Id = item.Id;
                memberDetail.ListDetail.Add(address);
            }
            return memberDetail;

        }
    }
}
