namespace ILBLI.Core.Log4
{
    /// <summary>
    /// 第三步：定义解析模板类
    /// </summary>
    public class EventLogLayout : log4net.Layout.PatternLayout
    {
        /// <summary>
        /// 这里的mylay是自定义的，需要和log4.config中配置的  <conversionPattern value="%mylay{EventCreateDate}"/> 相对应
        /// </summary>
        public EventLogLayout()
        {
            AddConverter("mylay", typeof(ParemterConverter));
        }
    }
}
