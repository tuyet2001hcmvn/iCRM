using ISD.API.EntityModels.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace ISD.API.Repositories.Marketing
{
    public class MemberOfTargetGroupRepository : GenericRepository<MemberOfTargetGroupModel>, IMemberOfTargetGroupRepository
    {
        public MemberOfTargetGroupRepository(ICRMDbContext context) : base(context)
        {
        }
        public IQueryable<ProfileModel> GetMemberOfTargetGroupById(Guid id, bool hasEmail, out int totalRow)
        {
            totalRow = 0;
            var listInfo = from member in context.MemberOfTargetGroupModels
                           join profile in context.ProfileModels on member.ProfileId equals profile.ProfileId
                           where (member.TargetGroupId == id) 
                           &&(hasEmail == false || (profile.Email != null && profile.Email != ""))
                           orderby (member.TargetGroupId) descending
                           select new ProfileModel()
                           {
                               ProfileId = profile.ProfileId,
                               ProfileCode = profile.ProfileCode,
                               ProfileForeignCode = profile.ProfileForeignCode,
                               ProfileName = profile.ProfileName,
                               Phone = profile.Phone,
                               Email = profile.Email
                           };
            //var listExternal = from member in context.MemberOfExternalProfileTargetGroupModels
            //                   where (member.TargetGroupId == id)
            //                   select new ProfileModel() {
            //                       ProfileId = new Guid("00000000-0000-0000-0000-000000000000"),
            //                       ProfileCode = -1,
            //                       ProfileForeignCode = null,
            //                       ProfileName = member.FullName,
            //                       Email = member.Email
            //                   };
            //var res = listInfo.ToList().Concat(listExternal.ToList()).AsQueryable();
            totalRow = listInfo.Count();
            return listInfo;
        }

        public IQueryable<ProfileModel> GetExternalMemberOfTargetGroupById(Guid id, out int totalRow)
        {
            totalRow = 0;     
            var listExternal = from member in context.MemberOfExternalProfileTargetGroupModels
                               where (member.TargetGroupId == id)
                               select new ProfileModel()
                               {
                                   ProfileId = new Guid("00000000-0000-0000-0000-000000000000"),
                                   ProfileCode = -1,
                                   ProfileForeignCode = null,
                                   ProfileName = member.FullName,
                                   Phone = member.Phone,
                                   Email = member.Email
                               };
            totalRow = listExternal.Count();
            return listExternal;
        }

        public int GetTotalMemberOfGroup(Guid id)
        {
            var member = context.MemberOfTargetGroupModels.Where(s => s.TargetGroupId == id);
            return member.Count();
        }

        public void Import(DataTable myTableType, Guid targetGroupId)
        {
            
            SqlParameter tableParam = new SqlParameter("@myTableType", myTableType);
            tableParam.TypeName = "ExcelMember";
            context.Database.ExecuteSqlRaw("exec Marketing.InsertMember @myTableType, @targetGroupId", tableParam, new SqlParameter("@targetGroupId", targetGroupId));
        }
    }
}

