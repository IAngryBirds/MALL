using System;
using System.Security.Cryptography;
using System.Text;

namespace ILBLI.Core
{
    #region 原版本

    ///// <summary>
    ///// AES加解密类
    ///// </summary>
    //public class AESHelper
    //{
    //    static AESHelper _AES;
    //    public string AESKey{ get; set;}
    //    public string AESIV{get; set;}
    //    private AESHelper()
    //    {
    //        AESKey = ConfigManager.GetConfigAppsettingValueByKey("AESKey");
    //        AESIV = ConfigManager.GetConfigAppsettingValueByKey("AESIV");
    //    }
    //    public static AESHelper AES
    //    {
    //        get
    //        {
    //            if (_AES == null)
    //            {
    //                _AES = new AESHelper();
    //            }
    //            return _AES;
    //        }
    //        set { _AES = value; }
    //    }

    //    /// <summary>
    //    /// AES加密
    //    /// </summary>
    //    /// <param name="textData">待加密字符</param>
    //    /// <returns>已加密字符串</returns>
    //    public string EncryptText(string textData)
    //    {
    //        string cryptText = string.Empty;
    //        EncryptText(textData, AESKey, AESIV, out cryptText);

    //        return cryptText;
    //    }

    //    /// <summary>
    //    /// AES解密
    //    /// </summary>
    //    /// <param name="cryptText">待解密字符串</param>
    //    /// <returns>已解密的字符串</returns>
    //    public string DecryptText(string cryptText)
    //    {
    //        string textData = string.Empty;
    //        DecryptText(cryptText, AESKey, AESIV, out textData);

    //        return textData;
    //    }

    //    /// <summary>
    //    /// AES加密 
    //    /// </summary>
    //    /// <param name="TextData">待加密字符</param>
    //    /// <param name="Key">加密密钥</param>
    //    /// <param name="IV">初始化向量</param>
    //    /// <param name="CryptText">输出:已加密字符串</param>
    //    /// <returns>0:成功加密 -1:待加密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 -4:其他错误</returns>
    //    private int EncryptText(string TextData, string Key, string IV, out string CryptText)
    //    {
    //        int keySize = 128;
    //        CryptText = "";

    //        int num = CheckParams(keySize, Key, IV, TextData);
    //        if (num != 0)
    //        {
    //            return num;
    //        }

    //        try
    //        {
    //            ICryptoTransform transform = GetRijndaelManaged(keySize, Key, IV).CreateEncryptor();
    //            byte[] plainText = Encoding.UTF8.GetBytes(TextData.Trim());
    //            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

    //            CryptText = Convert.ToBase64String(cipherBytes);

    //            return num;
    //        }
    //        catch (Exception e)
    //        {
    //            throw new Exception($"加密时发生异常，异常信息：{e.Message},字符串为：{TextData},Key:{Key},IV{IV}");
    //        }
    //    }
        
    //    /// <summary>
    //    /// AES解密
    //    /// </summary>
    //    /// <param name="CryptText">待解密字符串</param>
    //    /// <param name="Key">加密密钥</param>
    //    /// <param name="IV">初始化向量</param>
    //    /// <param name="TextData">输出:已解密的字符串</param>
    //    /// <returns>0:成功解密 -1:待解密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 -4:其他错误</returns>
    //    private int DecryptText(string CryptText, string Key, string IV, out string TextData)
    //    {
    //        int keySize = 128;

    //        TextData = "";
    //        int num = CheckParams(keySize, Key, IV, CryptText);
    //        if (num != 0)
    //        {
    //            return num;
    //        }

    //        try
    //        {
    //            ICryptoTransform transform = GetRijndaelManaged(keySize, Key, IV).CreateDecryptor();
    //            byte[] encryptedData = Convert.FromBase64String(CryptText);
    //            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

    //            TextData = Encoding.UTF8.GetString(plainText).Trim();
    //            return num;
    //        }
    //        catch (Exception e)
    //        {
    //            throw new Exception($"解密时发生异常，异常信息：{e.Message},字符串为：{CryptText},Key:{Key},IV{IV}");
    //        }
    //    }

    //    /// <summary>
    //    /// 定义一个基本的加密转换运算
    //    /// </summary>
    //    /// <param name="keySize"></param>
    //    /// <param name="key"></param>
    //    /// <param name="IV"></param>
    //    /// <returns></returns>
    //    private RijndaelManaged GetRijndaelManaged(int keySize, string key, string IV)
    //    {
    //        RijndaelManaged rijndaelCipher = new RijndaelManaged();

    //        rijndaelCipher.Mode = CipherMode.CBC;
    //        rijndaelCipher.Padding = PaddingMode.PKCS7;
    //        rijndaelCipher.KeySize = keySize;
    //        rijndaelCipher.BlockSize = keySize;

    //        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
    //        byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

    //        byte[] keyBytes = new byte[keySize / 8];
    //        int len = pwdBytes.Length;
    //        if (len > keyBytes.Length) len = keyBytes.Length;
    //        System.Array.Copy(pwdBytes, keyBytes, len);

    //        rijndaelCipher.Key = keyBytes;
    //        rijndaelCipher.IV = ivBytes;

    //        return rijndaelCipher;
    //    }

    //    /// <summary>
    //    /// 检查输入参数是否正确
    //    /// </summary>
    //    /// <param name="keySize"></param>
    //    /// <param name="key"></param>
    //    /// <param name="IV"></param>
    //    /// <param name="textData"></param>
    //    /// <returns>0:成功解密 -1:待解密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 </returns>
    //    private int CheckParams(int keySize, string key, string IV, string textData)
    //    {
    //        if (string.IsNullOrWhiteSpace(textData))
    //        {
    //            return -1;
    //        }
    //        if (string.IsNullOrWhiteSpace(key))
    //        {
    //            return -2;
    //        }
    //        if (string.IsNullOrWhiteSpace(IV) || IV.Length != keySize / 8)
    //        {
    //            return -3;
    //        }

    //        return 0;
    //    }
    //}
    #endregion

    #region 新版本--使用单例模式 modify: 20190730

    /// <summary>
    /// AES加解密类
    /// </summary>
    public class AESHelper
    {
        private AESHelper()
        {
            AESKey = ConfigManager.GetConfigAppsettingValueByKey("AESKey");
            AESIV = ConfigManager.GetConfigAppsettingValueByKey("AESIV");
        }

        private static readonly Lazy<AESHelper> lazy = new Lazy<AESHelper>(() => new AESHelper());

        public static AESHelper Instance
        {
            get { return lazy.Value; }
        }

        private static string AESKey { get; set; }
        private static string AESIV { get; set; }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="textData">待加密字符</param>
        /// <returns>已加密字符串</returns>
        public static string EncryptText(string textData)
        {
            string cryptText = string.Empty;
            EncryptText(textData, AESKey, AESIV, out cryptText);

            return cryptText;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cryptText">待解密字符串</param>
        /// <returns>已解密的字符串</returns>
        public static string DecryptText(string cryptText)
        {
            string textData = string.Empty;
            DecryptText(cryptText, AESKey, AESIV, out textData);

            return textData;
        }

        /// <summary>
        /// AES加密 
        /// </summary>
        /// <param name="TextData">待加密字符</param>
        /// <param name="Key">加密密钥</param>
        /// <param name="IV">初始化向量</param>
        /// <param name="CryptText">输出:已加密字符串</param>
        /// <returns>0:成功加密 -1:待加密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 -4:其他错误</returns>
        private static int EncryptText(string TextData, string Key, string IV, out string CryptText)
        {
            int keySize = 128;
            CryptText = "";

            int num = CheckParams(keySize, Key, IV, TextData);
            if (num != 0)
            {
                return num;
            }

            try
            {
                ICryptoTransform transform = GetRijndaelManaged(keySize, Key, IV).CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(TextData.Trim());
                byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                CryptText = Convert.ToBase64String(cipherBytes);

                return num;
            }
            catch (Exception e)
            {
                throw new Exception($"加密时发生异常，异常信息：{e.Message},字符串为：{TextData},Key:{Key},IV{IV}");
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="CryptText">待解密字符串</param>
        /// <param name="Key">加密密钥</param>
        /// <param name="IV">初始化向量</param>
        /// <param name="TextData">输出:已解密的字符串</param>
        /// <returns>0:成功解密 -1:待解密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 -4:其他错误</returns>
        private static int DecryptText(string CryptText, string Key, string IV, out string TextData)
        {
            int keySize = 128;

            TextData = "";
            int num = CheckParams(keySize, Key, IV, CryptText);
            if (num != 0)
            {
                return num;
            }

            try
            {
                ICryptoTransform transform = GetRijndaelManaged(keySize, Key, IV).CreateDecryptor();
                byte[] encryptedData = Convert.FromBase64String(CryptText);
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                TextData = Encoding.UTF8.GetString(plainText).Trim();
                return num;
            }
            catch (Exception e)
            {
                throw new Exception($"解密时发生异常，异常信息：{e.Message},字符串为：{CryptText},Key:{Key},IV{IV}");
            }
        }

        /// <summary>
        /// 定义一个基本的加密转换运算
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static RijndaelManaged GetRijndaelManaged(int keySize, string key, string IV)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = keySize;
            rijndaelCipher.BlockSize = keySize;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            byte[] keyBytes = new byte[keySize / 8];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = ivBytes;

            return rijndaelCipher;
        }

        /// <summary>
        /// 检查输入参数是否正确
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <param name="textData"></param>
        /// <returns>0:成功解密 -1:待解密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 </returns>
        private static int CheckParams(int keySize, string key, string IV, string textData)
        {
            if (string.IsNullOrWhiteSpace(textData))
            {
                return -1;
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                return -2;
            }
            if (string.IsNullOrWhiteSpace(IV) || IV.Length != keySize / 8)
            {
                return -3;
            }

            return 0;
        }
    }

    #endregion
}
