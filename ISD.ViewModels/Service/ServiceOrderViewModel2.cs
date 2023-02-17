using AutoMapper;
using AutoMapper.Configuration;
using ISD.Core;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels.Service
{
    public class ServiceOrderViewModel2 : IHaveCustomMappings
    {
        //public string UserName { get; set; }
        //public int Enhancements { get; set; }
        //public int Bugs { get; set; }
        //public int Support { get; set; }
        //public int Other { get; set; }

        public string SaleOrgName { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //configuration.CreateMap<ServiceOrderModel, ServiceOrderViewModel2>()
                //.ForMember(m => m.SaleOrgName, opt =>
                    //opt.MapFrom(u => 
            //    .ForMember(m => m.Bugs, opt =>
            //        opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Bug)))
            //    .ForMember(m => m.Support, opt =>
            //        opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Support)))
            //    .ForMember(m => m.Other, opt =>
            //        opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Other)));
        }
    }
}
