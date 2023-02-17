using AutoMapper;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Work.Infrastructure.Mapping
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<WorkFlowModel, WorkFlowViewModel>();
        }

        public static void Run()
        {
            Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperConfiguration>();
            });
        }
    }
}