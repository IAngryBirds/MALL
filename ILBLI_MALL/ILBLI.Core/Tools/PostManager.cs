using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ILBLI.Core
{
    public class PostManager
    {
        /// <summary>
        /// 接口数据的POST提交
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            string strValue = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method ="POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] payload = Encoding.UTF8.GetBytes(param);
            request.ContentLength = payload.Length;
            try
            {
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream s = response.GetResponseStream())
                        {
                            string StrDate = "";
                            using (StreamReader Reader = new StreamReader(s, Encoding.UTF8))
                            {
                                while ((StrDate = Reader.ReadLine()) != null)
                                {
                                    strValue += StrDate + "\r\n";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return strValue;
        }

        /// <summary>
        /// POST提交JSON数据
        /// </summary>
        /// <param name="url">请求Url地址</param>
        /// <param name="param">请求参数</param> 
        /// <param name="dic">请求头部参数</param>
        /// <returns></returns>
        public static string PostJson(string url, string param, Dictionary<string, string> dic = null)
        {
            string strValue = "";
            string strURL = url;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.Accept = "application/json;charset=UTF-8";
            if (dic != null && dic.Count > 0)
            {
                foreach (var item in dic)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            byte[] payload = Encoding.UTF8.GetBytes(param);
            request.ContentLength = payload.Length;
            try
            {
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream s = response.GetResponseStream())
                        {
                            string StrDate = "";
                            using (StreamReader Reader = new StreamReader(s, Encoding.UTF8))
                            {
                                while ((StrDate = Reader.ReadLine()) != null)
                                {
                                    strValue += StrDate;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("connection_timed_out");
            }
            return strValue;
        }

        /// <summary>
        /// POST提交数据下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static byte[] PostData(string url, string param)
        {
            string strURL = url;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.Accept = "*/*";
            byte[] payload = Encoding.UTF8.GetBytes(param);
            request.ContentLength = payload.Length;
            try
            {
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        byte[] buffer = new byte[response.ContentLength];
                        using (Stream s = response.GetResponseStream())
                        {
                            int sign = 0;
                            int count = 0;
                            int length = buffer.Length;
                            while (length > 0)
                            {
                                count = s.Read(buffer, sign, length);
                                length -= count;
                                sign += count;
                            }
                            return buffer;
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("connection_timed_out");
            }
        }
 

        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="str">string str:被加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string SHA256Encrypt(string str)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(str);
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

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        { 
            return (DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000;
        }

    }
}
