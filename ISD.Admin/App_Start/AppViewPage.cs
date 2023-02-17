using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using ISD.ViewModels;
using System.Web.Mvc;
using System.Configuration;

namespace ISD.Admin.App_Start
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }
        protected string Net5ApiDomain
        {          

            get
            {
                return ConfigurationManager.AppSettings["APINET5DomainUrl"];
            }
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}