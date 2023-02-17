using ISD.API.Core;
using ISD.API.EntityModels.Entities;
using ISD.API.ViewModels.MarketingViewModels.MemberOfTargetGroupViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ISD.API.Repositories.Services.Marketing
{
    public class MemberOfTargetGroupService :IMemberOfTargetGroupService
    {
        private readonly UnitOfWork _unitOfWork;
        public MemberOfTargetGroupService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<MemberOfTargetGroupViewViewModel> GetExternalMemberOfTargetGroupById(Guid id, int pageIndex, int pageSize, out int totalRow)
        {
           
            //  var s = _unitOfWork.MemberOfTargetGroupRepository.GetMemberOfTargetGroupById(id, hasEmail).ToList();
            List<MemberOfTargetGroupViewViewModel> listInfo = _unitOfWork.MemberOfTargetGroupRepository
                .GetExternalMemberOfTargetGroupById(id, out totalRow).Skip(pageIndex * pageSize - pageSize).Take(pageSize)
                .Select(m => (new MemberOfTargetGroupViewViewModel
                {
                    ProfileId = m.ProfileId,
                    ProfileCode = m.ProfileCode.ToString(),
                    ProfileForeignCode = m.ProfileForeignCode,
                    ProfileName = m.ProfileName,
                    Phone = m.Phone,
                    Email = m.Email
                })).ToList();

            return listInfo;
        }
        public List<MemberOfTargetGroupViewViewModel> GetMemberOfTargetGroupById(Guid id, bool hasEmail, bool distinctEmail, int pageIndex, int pageSize, out int totalRow)
        {
            if (distinctEmail)
            {

                List<MemberOfTargetGroupViewViewModel> listInfo1 = _unitOfWork.MemberOfTargetGroupRepository.GetMemberOfTargetGroupById(id, hasEmail, out totalRow).DistinctBy(p => p.Email).Skip(pageIndex * pageSize - pageSize).Take(pageSize)
               .Select(m => (new MemberOfTargetGroupViewViewModel
               {
                   ProfileId = m.ProfileId,
                   ProfileCode = m.ProfileCode.ToString(),
                   ProfileForeignCode = m.ProfileForeignCode,
                   ProfileName = m.ProfileName,
                   Phone = m.Phone,
                   Email = m.Email
               })).ToList();
                return listInfo1;
            }
          //  var s = _unitOfWork.MemberOfTargetGroupRepository.GetMemberOfTargetGroupById(id, hasEmail).ToList();
            List<MemberOfTargetGroupViewViewModel> listInfo = _unitOfWork.MemberOfTargetGroupRepository
                .GetMemberOfTargetGroupById(id, hasEmail, out totalRow).Skip(pageIndex * pageSize - pageSize).Take(pageSize)
                .Select(m => (new MemberOfTargetGroupViewViewModel
                {
                    ProfileId = m.ProfileId,
                    ProfileCode = m.ProfileCode.ToString(),
                    ProfileForeignCode = m.ProfileForeignCode,
                    ProfileName = m.ProfileName,
                    Phone = m.Phone,
                    Email = m.Email
                })).ToList();
           
            return listInfo;
        }
        public void Import(IFormFile file, Guid targetGroupId, string type = null)
        {
            ExcelHelper excelHelper = new ExcelHelper(file);
            DataSet dataSet = excelHelper.GetDataSet();

            #region Read data from file
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0].Columns[1].Table;
            dt.TableName = "ExcelMember";

            dt.Columns.Remove("Column0");
            dt.Columns.Remove("Column2");
            for(int i=0;i<5;i++)
            {
                DataRow dr = dt.Rows[i];
                dr.Delete();
            } 
            dt.AcceptChanges();
            dt.Columns[0].ColumnName = "ProfileId";
            dt.Columns[1].ColumnName = "Email";
            #endregion

            #region Lọc dữ liệu
            List<MemberOfTargetGroupImportViewModel> listMember = (from DataRow row in dt.Rows

                                                                     select new MemberOfTargetGroupImportViewModel
                                                                     {
                                                                         ProfileId = row["ProfileId"].ToString(),
                                                                         Email = row["Email"].ToString()

                                                                     }).ToList();
            
            var filterList = listMember;
            //Không lọc đối với trường hợp chiến dịch quản bá sản phẩm
            if (type != "ProductPromotion")
            {
                //Lọc theo điều kiện email khác null, khác rỗng, chứa ký tự '@' và không lấy trùng
                filterList = listMember.Where(s => s.Email != null && s.Email != "" && s.Email.Contains("@")).DistinctBy(s => s.Email).ToList();
            }
        
            #endregion

            #region Add dữ liệu đã lọc vào new data table
            DataTable dt1 = new DataTable();
            dt1.TableName = "ExcelMember";
            dt1.Columns.Add("ProfileId");
            foreach(var item in filterList)
            {
                dt1.Rows.Add(item.ProfileId);
            }
            #endregion

            _unitOfWork.MemberOfTargetGroupRepository.Import(dt1, targetGroupId);
        } 
        public void ImportExternal(IFormFile file, Guid targetGroupId)
        {
            ExcelHelper excelHelper = new ExcelHelper(file);
            DataSet dataSet = excelHelper.GetDataSet();
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0].Columns[1].Table;
            for (int i = 5; i < dt.Rows.Count; i++)
            {
                var name = dt.Rows[i][2].ToString();
                var phone = dt.Rows[i][3].ToString();
                var email = dt.Rows[i][4].ToString();
                var existing = _unitOfWork.MemberOfExternalProfileTargetGroupRepository.GetBy(s => s.FullName == name && s.Email == email && s.Phone == phone && s.TargetGroupId == targetGroupId);
                if (existing == null)
                {
                    MemberOfExternalProfileTargetGroupModel newMember = new MemberOfExternalProfileTargetGroupModel();
                    newMember.ExternalProfileTargetGroupId = Guid.NewGuid();
                    newMember.FullName = name;
                    newMember.Phone = phone;
                    newMember.Email = email;
                    newMember.TargetGroupId = targetGroupId;
                    _unitOfWork.MemberOfExternalProfileTargetGroupRepository.Add(newMember);
                }
            }
            _unitOfWork.Save();
        }

    }
}
