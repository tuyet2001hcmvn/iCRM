using AutoMapper;
using AutoMapper.Configuration;
using ISD.Core;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ISD.Admin.App_Start
{
    public class AutoMapperConfig : IRunAtInit
    {
        public static MapperConfigurationExpression Configuration { get; } = new MapperConfigurationExpression();

        public void Execute()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes().ToList();
            //var types2 = Assembly.GetExecutingAssembly().GetReferencedAssemblies();


            foreach (var assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Assembly assembly = Assembly.Load(assemblyName);
                foreach (var type in assembly.GetTypes())
                {
                    types.Add(type);
                }
            }

            LoadStandardMappings(types);

            LoadCustomMappings(types);

            //Configuration.CreateMap<ServiceDetailDTO, ServiceOrderDetailServiceModel>();
            //Configuration.CreateMap<ServiceDetailDTO, ServiceOrderDetailAccessoryModel>();
            //Configuration.CreateMap<ServiceOrderViewModel, ServiceOrderModel>()
            //        // Khai báo ở đây hoặc sử dụng @Html.HiddenFor(p => p.ServiceOrder.StoreName) để nó không map từ VM sang Model
            //        .ForMember(dest => dest.ServiceOrderId, source => source.Ignore())
            //        .ForMember(dest => dest.ServiceOrderName, source => source.Ignore())
            //        .ForMember(dest => dest.StoreName, source => source.Ignore())
            //        .ForMember(dest => dest.IsNew, source => source.Ignore())
            //        .ForMember(dest => dest.GeneratedCode, source => source.Ignore()
            //    ); 

            Mapper.Initialize(Configuration);

        }

        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(Configuration);
            }

        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType &&
                              i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t
                        }).ToArray();

            foreach (var map in maps)
            {
                Configuration.CreateMap(map.Source, map.Destination);
            }
        }
    }
}