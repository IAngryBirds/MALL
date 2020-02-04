using System.Web;

namespace ILBLI.Core
{
    public class HttpContextExtend
    {
        public string ClientIP { get; set; }
        public string ClientUserAgent { get; set; }
        public string ClientUrl { get; set; }

        public HttpContextExtend()
        {
            this.ClientIP = GetClientIP();
            this.ClientUserAgent = GetClientUserAgent();
            this.ClientUrl = GetClientRawUrl();
        }

        /// <summary>
        /// 此次请求内容简述
        /// </summary>
        /// <returns></returns>
        public string HttpRequestMessage()
        {
            return string.Format("此次请求IP[{0}],请求来源[{1}],请求地址[{2}]", this.ClientIP, this.ClientUserAgent, this.ClientUrl);
        }

        /// <summary>
        /// 获取客户端IP信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetClientIP()
        {
            string ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取客户端头部信息
        /// </summary>
        /// <returns></returns>
        private string GetClientUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// 获取请求路径
        /// </summary>
        /// <returns></returns>
        private string GetClientRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }
    }


}
