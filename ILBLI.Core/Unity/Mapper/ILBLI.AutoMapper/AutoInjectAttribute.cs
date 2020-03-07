using System;

namespace ILBLI.Unity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AutoInjectAttribute : Attribute
    {
        public Type SourceType { get; }
        public Type TargetType { get; }

        public AutoInjectAttribute(Type sourceType, Type targetType)
        {
            SourceType = sourceType;
            TargetType = targetType;
        }
    }
}
