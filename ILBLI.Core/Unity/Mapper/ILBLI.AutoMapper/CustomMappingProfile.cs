using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ILBLI.Unity
{
    public class CustomMappingProfile : Profile
    {
        /// <summary>
        /// 需要注入的Assembly程序集【可以写成配置文件加载】
        /// </summary>
        private static readonly List<string> _LibNameList = new List<string>() { "ILBLI.Model" };

        public CustomMappingProfile()
        {
            //可配置到配置文件中
            List<Assembly> assemblys = new List<Assembly>();
            foreach (var libName in _LibNameList)
            {
                assemblys.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(libName)));
            }

            AutoInjectAttribute attribute;
            assemblys.ForEach(x => {
                Type[] types = x.GetTypes();
                var attributes = types?.Where(t => t.IsDefined(typeof(AutoInjectAttribute), true) && !t.IsAbstract)
                                      ?.Select(t => t.GetCustomAttributes(typeof(AutoInjectAttribute), true));

                if (attributes == null || !attributes.Any())
                    return;
                 
                foreach (var auto in attributes)
                {
                    attribute = (AutoInjectAttribute)auto?[0];

                    if (attribute != null)
                    {
                        CreateMap(attribute.SourceType, attribute.TargetType)
                            .ReverseMap()
                            .ForAllMembers(opt => opt.Condition((src, desc, sourceMember) => sourceMember != null));
                    }
                } 
            }); 
        }

    }
}
