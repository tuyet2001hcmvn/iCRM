using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ISD.Repositories.Customer
{
    public class ProfileEmailRepository
    {
        private EntityDataContext _context;

        public ProfileEmailRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật email
        /// </summary>
        /// <param name="email">Danh sách email</param>
        /// <param name="profileId">ProfileId</param>
        /// <returns>True||False</returns>
        public bool UpdateEmail(List<string> email, Guid? profileId, out string errMess)
        {
            errMess = string.Empty;
            try
            {
                //Lấy ra danh sách email bởi profileId
                var listEmailInDb = _context.ProfileEmailModel.Where(p => p.ProfileId == profileId).ToList();

                //Nếu có rồi thì update
                if (listEmailInDb != null && listEmailInDb.Count > 0)
                {
                    //Xoá cái cũ
                    for (int i = listEmailInDb.Count - 1; i >= 0; i--)
                    {
                        _context.Entry(listEmailInDb[i]).State = EntityState.Deleted;
                    }
                    //thêm lại
                    foreach (string e in email)
                    {
                        var pAdd = e?.Trim().Replace(" ", "");
                        if (pAdd != string.Empty)
                        {
                            //Check định dạng email
                            if (IsValidEmail(pAdd) == false)
                            {
                                errMess = "Email: \"" + pAdd + "\" không đúng định dạng";
                                return false;
                            }
                            var emailAdd = new ProfileEmailModel
                            {
                                EmailId = Guid.NewGuid(),
                                ProfileId = profileId,
                                Email = pAdd
                            };
                            _context.Entry(emailAdd).State = EntityState.Added;
                        }
                    }
                }
                else
                {
                    //thêm mới
                    foreach (var e in email)
                    {
                        var pAdd = e.Trim();
                        if (pAdd != string.Empty)
                        {
                            //Check định dạng email
                            if (IsValidEmail(pAdd) == false)
                            {
                                errMess = "Email: \"" + pAdd + "\" không đúng định dạng";
                                return false;
                            }
                            var emailAdd = new ProfileEmailModel
                            {
                                EmailId = Guid.NewGuid(),
                                ProfileId = profileId,
                                Email = pAdd
                            };
                            _context.Entry(emailAdd).State = EntityState.Added;
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

        private bool IsValidEmail(string email)
        {
            //try
            //{
            //    var addr = new MailAddress(email);
            //    return addr.Address == email;
            //}
            //catch
            //{
            //    return false;
            //}
            string RegexPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
                                          @"@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            return Regex.IsMatch(email, RegexPattern, RegexOptions.IgnoreCase);
        }
    }
}
