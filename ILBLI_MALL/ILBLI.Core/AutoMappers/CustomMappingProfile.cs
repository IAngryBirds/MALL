using AutoMapper; 

namespace ILBLI.Core
{
    /// <summary>
    /// AutoMapper提供了个性化设置Profile，使得我们转换后的数据格式可以多变，//配置类放到BaseBll类层
    /// 当然还可以配置全局格式等等，需要继承自Profile，并重写Configure方法，然后在AutoMapper初始化的时候
    /// </summary>
    public class CustomMappingProfile : Profile
    {
        public CustomMappingProfile()
        {
            CustomCreateMapping();
        }

        /// <summary>
        /// 在方法中写自定义的类型转换方法CreateMap<User, ILBLI_User>().ReverseMap();
        /// </summary>
        private void CustomCreateMapping()
        {
            //CreateMap<User, ILBLI_User>().ReverseMap();//User用户信息自动映射
            //.ReverseMap().ForAllMembers(opt => opt.Condition((src, desc, sourceMember) => sourceMember != null)); 用来过滤null值转换
        }

    }
}
