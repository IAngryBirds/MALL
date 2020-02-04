using log4net;

namespace ILBLI.Core
{
    public class LogNHelper
    {
        private ILog log { get; set; }

        #region 初始化日志类-返回ILog 第二套 
         
        public LogNHelper(string configStr = "sysLog")
        {
            log = LogManager.GetLogger(configStr);
        }

        public ILog Instance
        {
            get { return log; }
        }

        #endregion


    }
}
