using ISD.Constant;
using ISD.EntityModels;
using ISD.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class SendSMSRepository
    {
        private EntityDataContext _context;
        public SendSMSRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public SMSModel SendSMSToCustomer(SendSMSViewModel smsViewModel)
        {
            string restClient = string.Format("{0}/SendSMS", ConstDomain.DomainSMSAPI);
            var client = new RestClient(restClient);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("token", "<token được cấp>");
            request.AddHeader("token", smsViewModel.Token);

            //request.AddHeader("Accept", "application/json");
            //request.AddHeader("Authorization", ConstDomain.TokenSMSAPI);

            //================================================================================
            //Parameters
            //1. to: Người nhận tin
            //2. type = 1: Loại tin cần gửi - chăm sóc khách hàng
            //3. from: Brandname dùng để gửi tin (dưới 150 ký tự)
            //4. message: Nội dung tin cần gửi
            //5. scheduled: Gửi tin đặt lịch 
            // -------------- Thiết lập giờ gửi theo định dạng dd-MMyyyy HH24:mm +/ -HH:mm 
            // -------------- ví dụ 15-01-2019 16:05 hoặc 15-01-2019 16:05 + 07:00, +/ -HH:mm là Time zone bỏ trống mặc định là + 07:00 Bangkok, HaNoi, Jakarta. 
            // -------------- Để trống trường scheduled ("scheduled":""), tin sẽ được gửi luôn sau khi nhận thành công.
            //6. requestId: ID định danh của đối tác, sẽ gửi lại trong nội dung phản hồi hoặc để trống("requestId","")
            //7. useUnicode: 
            // -------------- 0: không unicode
            // -------------- 1: gửi tin unicode
            // -------------- 2: tự động chuyển đổi nội dung sang tiếng việt không dấu
            string content = string.Format("{0}\n \"to\": \"{1}\",\n \"from\": \"{2}\",\n \"message\": \"{3}\",\n \"scheduled\": \"\",\n \"requestId\": \"\",\n \"useUnicode\": 2,\n \"type\": 1,\n{4}",
                                           "{",
                                           smsViewModel.PhoneNumber,
                                           smsViewModel.BrandName,
                                           smsViewModel.Message,
                                           "}");
            request.AddParameter("application/json",
                                //"{\n \"to\": \"09*******\",\n \"from\": \"* \",\n \"message\": \"Noi dung gui\",\n \"scheduled\": \"\",\n \"requestId\": \"\",\n \"useUnicode\": 0,\n \"type\": 1,\n}", 
                                content,
                                ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            //Save DB
            SMSModel sms = new SMSModel();
            sms.SMSId = Guid.NewGuid();
            sms.SMSContent = smsViewModel.Message;
            sms.SMSTo = smsViewModel.PhoneNumber;
            sms.BrandName = smsViewModel.BrandName;
            sms.ResponseText = response.Content;
            sms.isSent = response.IsSuccessful == true ? true : false;
            sms.CreateDate = DateTime.Now;

            if (response.IsSuccessful == false)
            {
                try
                {
                    dynamic result = JsonConvert.DeserializeObject(response.Content);
                    sms.ErrorMessage = result.errorMessage;

                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    sms.ErrorMessage = Regex.Replace(response.Content, "<.*?>", string.Empty).Replace("\n\n", "").Replace("\n", ". ");
                }
            }

            _context.Entry(sms).State = System.Data.Entity.EntityState.Added;
            return sms;
            //Console.WriteLine(response.Content);
        }
    }
}
