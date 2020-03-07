using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ILBLI.Tool
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// 调用用户中心接口获取用户数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static JObject GetDataByHttpRequest(string url, RequestMethod method = RequestMethod.GET)
        {
            JObject obj = new JObject();
            Encoding myEncoding = Encoding.GetEncoding("UTF-8");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToString();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), myEncoding))
            {
                string result = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(result))
                {
                    obj = JObject.Parse(result);
                }
            }
            return obj;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000;
        }

        /// <summary>
        /// 获取当前时间戳[毫秒]
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampMillisecond()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
        }

        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA256Encrypt(string str)
        {
            byte[] bytValue = Encoding.UTF8.GetBytes(str);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(bytValue);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("SHA256Hash() fail,error:" + ex.Message);
            }
        }


    }
}
