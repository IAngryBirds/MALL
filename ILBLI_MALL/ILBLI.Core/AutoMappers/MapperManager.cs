using AutoMapper;
using System.Collections;
using System.Collections.Generic;

namespace ILBLI.Core
{
    public class MapperManager
    {
        static MapperManager()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CustomMappingProfile>());//AutoMapper的自定义初始化
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(object source) where TDestination : class, new()
        {
            if (source == null)
            {
                return default(TDestination);
            }

            return Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 实体集合映射
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TDestination>(IEnumerable source) where TDestination : class, new()
        {
            if (source == null)
            {
                return default(IEnumerable<TDestination>);
            }

            return Mapper.Map<IEnumerable<TDestination>>(source);
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source) where TSource : class, new() where TDestination : class, new()
        {
            if (source == null)
            {
                return default(TDestination);
            }

            return Mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        /// 实体集合映射
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source) where TSource : class, new() where TDestination : class, new()
        {
            if (source == null)
            {
                return default(IEnumerable<TDestination>);
            }

            return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }


    }
}
