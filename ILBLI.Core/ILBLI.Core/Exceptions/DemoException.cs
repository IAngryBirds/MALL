using System;

namespace ILBLI.Unity 
{
    /// <summary>
    /// 自定义错误信息示例
    /// </summary>
    public class DemoException:Exception
    {
        public override string Message => "自定义的错误信息示例";
    }
}
