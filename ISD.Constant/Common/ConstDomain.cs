using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ISD.Constant
{
    public class ConstDomain
    {
        //Test
        //public const string Domain = "http://192.168.0.105:3000";
        //Thật
        public static string Domain = WebConfigurationManager.AppSettings["DomainUrl"].ToString();
        public static string DomainImageCustomerPromotion = Domain + "/Upload/CustomerPromotion/thum";
        public static string DomainImageCustomerGift = Domain + "/Upload/Gift/thum";
        public static string DomainImageParentCategory = Domain + "/Upload/Brand/thum";
        public static string DomainImageCategory = Domain + "/Upload/Category/thum";
        public static string DomainImageProduct = Domain + "/Upload/Color/thum";
        public static string DomainBanner = Domain + "/Upload/Banner/thum";
        public static string DomainImageAccessoryCategory = Domain + "/Upload/AccessoryCategory/thum";
        public static string DomainImageStore = Domain + "/Upload/Store/Image/thum";
        public static string DomainLogoStore = Domain + "/Upload/Store/Logo/thum";
        public static string DomainImageAccessory = Domain + "/Upload/Accessory/thum";
        public static string DomainPeriodicallyChecking = Domain + "/Upload/PeriodicallyChecking/thum";
        public static string DomainPeriodicallyCheckingAPI = Domain + "/Upload/PeriodicallyChecking/";
        public static string DomainImageDefaultProduct = Domain + "/Upload/Color/thum";
        //Icon
        public static string DomainIcon = Domain + "/Upload/MaterialGroup/thum";
        //Hình loại xe
        public static string DomainProductHierarchy = Domain + "/Upload/ProductHierarchy/thum";
        //Hình sản phẩm (Material)
        public static string DomainMaterial = Domain + "/Upload/Material/thum";
        //Hình mô tả sản phẩm (MaterialDescription)
        public static string DomainMaterialDescription = Domain + "/Upload/MaterialDescription/thum";
        //noimage
        public static string NoImageUrl = Domain + "/Upload/noimage.jpg?v=1";
        public const string NoImage = "/Upload/noimage.jpg";

        public const string UrgentServiceUrl = "/Service/ServiceOrder/Edit/";
        public const string UrgentService_AccessorySaleOrderUrl = "/Sale/AccessorySaleOrder/Edit/";

        //doamin API
        public static string DomainAPI = WebConfigurationManager.AppSettings["APIDomainUrl"].ToString();

        //token, key 
        public const string tokenConst = "72E948C6DC62F17CBC169B7D2FA9F";
        public const string keyConst = "b564e199-1b94-41b7-b7c2-e1e080162f8a";

        //public const string tokenConst_New = "789673D8A277BFFB8892F37543FD3";
        //public const string keyConst_New = "813f9f01-1e85-4e8f-af5b-721a5a2d4029";

        public const string tokenConst_New = "CFD365BAD93A7726F6BADC1898AB4";
        public const string keyConst_New = "3adf078d-b2f4-4833-9143-bb68ff6ff41c";

        //domain SMS API VMG
        public static string DomainSMSAPI = WebConfigurationManager.AppSettings["DomainSMSAPI"].ToString();
        //Token cho brandname
        public static string TokenAnCuong = WebConfigurationManager.AppSettings["TokenAnCuong"].ToString();
        public static string TokenMalloca = WebConfigurationManager.AppSettings["TokenMalloca"].ToString();
        public static string TokenAconcept = WebConfigurationManager.AppSettings["TokenAconcept"].ToString();

        public static bool isSentSMS = bool.Parse(WebConfigurationManager.AppSettings["isSentSMS"]);

        public static string DocumentDomain = WebConfigurationManager.AppSettings["DocumentDomain"].ToString();

    }
}
