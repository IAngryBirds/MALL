using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILBLI.Unity 
{
    public static class UseAutoMapperInit
    {

        public static IServiceCollection AddAutoMapperInit(this IServiceCollection services)
        {
            IConfigurationProvider provider = new MapperConfiguration(cfg => cfg.AddProfile<CustomMappingProfile>());

            services.AddSingleton(provider);
            services.AddSingleton<IMapper, Mapper>();

            return services;
        }
    }
}
