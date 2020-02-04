using log4net.Core;
using log4net.Layout.Pattern;
using System.IO;

namespace ILBLI.Core.Log4
{
    /// <summary>
    /// 第二步：定义字段转换解析函数
    /// </summary>
    public class ParemterConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            LogModel eventLog = loggingEvent.MessageObject as LogModel;
            if (eventLog == null)
                writer.Write("");
            else
            {
                switch (Option.ToLower())
                {
                    case "eventlog":
                        writer.Write(eventLog.EventLog);
                        break;
                    case "ipaddress":
                        writer.Write(eventLog.IPAddress);
                        break;
                    case "operationtype":
                        writer.Write(eventLog.OperationType);
                        break;
                    case "operationsystem":
                        writer.Write(eventLog.OperationSystem);
                        break;
                    case "operationmsg":
                        writer.Write(eventLog.OperationMsg);
                        break;
                    case "eventcreatedate":
                        writer.Write(eventLog.EventCreateDate);
                        break;
                    default:
                        writer.Write("");
                        break;
                }
            }
        }

    }
}
