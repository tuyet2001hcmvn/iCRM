using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ISD.ViewModels
{
    public class AppUserPrincipal : ClaimsPrincipal
    {
        public AppUserPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {
        }
        #region Account info
        public Guid? AccountId
        {
            get
            {
                //return this.FindFirst(ClaimTypes.Sid).Value;
                try
                {
                    return Guid.Parse(this.FindFirst(ClaimTypes.Sid).Value);
                }
                catch
                {
                    return null;
                }
            }
        }
        public string UserName
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }
        public string EmployeeCode
        {
            get
            {
                try
                {
                    return this.FindFirst(ClaimTypes.Spn).Value;
                }
                catch
                {
                    return null;
                }
            }
        }
        public string FullName
        {
            get
            {
                try
                {
                    return this.FindFirst(ClaimTypes.Upn).Value;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
        #region Company info
        public Guid? CompanyId
        {
            get
            {
                try
                {
                    return Guid.Parse(this.FindFirst(ClaimTypes.Country).Value);
                }
                catch
                {
                    return null;
                }
            }
        }
        public string CompanyCode
        {
            get
            {
                try
                {
                    return this.FindFirst(ClaimTypes.System).Value;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

        public string SaleOrg
        {
            get
            {
                try
                {
                    return this.FindFirst(ClaimTypes.GivenName).Value;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string SaleOrgName
        {
            get
            {
                try
                {
                    return this.FindFirst(ClaimTypes.Surname).Value;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string RolesName
        {
            get
            {
                return this.FindFirst(ClaimTypes.Role).Value;
            }
        }

        public string RolesCodeList
        {
            get
            {
                try
                {
                    return this.FindFirst(ClaimTypes.HomePhone).Value;
                }
                catch //(Exception ex)
                {
                    return null;
                }
                
            }
        }

        public bool isShowChoseModule
        {
            get
            {
                if (this.FindFirst(ClaimTypes.Webpage) != null)
                {
                    return (this.FindFirst(ClaimTypes.Webpage).Value == "True");
                }
                else
                {
                    return false;
                }
            }
        }
        public bool isShowDashboard
        {
            get
            {
                if (this.FindFirst(ClaimTypes.WindowsAccountName) != null)
                {
                    return (this.FindFirst(ClaimTypes.WindowsAccountName).Value == "True");
                }
                else
                {
                    return false;
                }
            }
        }
        public bool isViewByStore
        {
            get
            {
                if (this.FindFirst(ClaimTypes.UserData) != null)
                {
                    return (this.FindFirst(ClaimTypes.UserData).Value == "True");
                }
                else
                {
                    return false;
                }
            }
        }

        //public string OneSignalWebId
        //{
        //    get
        //    {
        //        return this.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    }
        //}
    }
}
