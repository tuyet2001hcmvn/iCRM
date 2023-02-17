using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ISD.Repositories.Customer
{
    public class ProfilePhoneRepository
    {
        private EntityDataContext _context;

        public ProfilePhoneRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật số điện thoại
        /// </summary>
        /// <param name="phoneNumber">Danh sách số điện thoại</param>
        /// <param name="profileId">ProfileId</param>
        /// <returns>True||False</returns>
        public bool UpdatePhone(List<string> phoneNumber, Guid? profileId, out string errMess)
        {
            errMess = string.Empty;
            try
            {
                //Lấy ra danh sách số dt bởi profileId
                var listPhoneInDb = _context.ProfilePhoneModel.Where(p => p.ProfileId == profileId).ToList();

                //Nếu có rổi thì update
                if (listPhoneInDb != null && listPhoneInDb.Count > 0)
                {
                    //Xoá cái cũ
                    for (int i = listPhoneInDb.Count - 1; i >= 0; i--)
                    {
                        _context.Entry(listPhoneInDb[i]).State = EntityState.Deleted;
                    }
                    //thêm lại
                    foreach (string phone in phoneNumber)
                    {
                        var pAdd = phone?.Trim().Replace(" ","");
                        if (pAdd != string.Empty)
                        {
                            //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                            if (!pAdd.StartsWith("0") || pAdd.Length < 10 || pAdd.Length >= 15 || (pAdd.Length == 3 && pAdd.StartsWith("1")))
                            {
                                errMess = "SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)";
                                return false;
                            }
                            var phoneAdd = new ProfilePhoneModel
                            {
                                PhoneId = Guid.NewGuid(),
                                ProfileId = profileId,
                                PhoneNumber = pAdd
                            };
                            _context.Entry(phoneAdd).State = EntityState.Added;
                        }
                    }
                }
                else
                {
                    //thêm mới
                    foreach (var phone in phoneNumber)
                    {
                        var pAdd = phone.Trim();
                        if (pAdd != string.Empty)
                        {
                            //Check SĐT liên hệ nhập sai định dạng: phải bắt đầu bằng số 0, >= 10 số, < 15 số
                            if (!pAdd.StartsWith("0") || pAdd.Length < 10 || pAdd.Length >= 15 || (pAdd.Length == 3 && pAdd.StartsWith("1")))
                            {
                                errMess = "SĐT liên hệ không đúng định dạng! (phải bắt đầu bằng số 0, >= 10 số, < 15 số)";
                                return false;
                            }
                            var phoneAdd = new ProfilePhoneModel
                            {
                                PhoneId = Guid.NewGuid(),
                                ProfileId = profileId,
                                PhoneNumber = pAdd
                            };
                            _context.Entry(phoneAdd).State = EntityState.Added;
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
    }
}