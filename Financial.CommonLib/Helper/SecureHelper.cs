using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financial.CommonLib
{
    /// <summary>
    /// 安全(加密等)
    /// </summary>
    public class SecureHelper
    {
        /// <summary>
        /// MD5加密(16位)
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>MD5加密(16位)后的字符串</returns>
        public static string EncryptByMD5(string str)
        {
            try
            {
                string result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
                result = result.Substring(8, 16);
                return result;
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// MD5加密(32位)
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>MD5加密(32位)后的字符串</returns>
        public static string EncryptByMD5_32(string str)
        {
            try
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string EncryptBySHA1(string str)
        {
            byte[] StrRes = Encoding.Default.GetBytes(str);
            System.Security.Cryptography.HashAlgorithm iSHA = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// 对URL参数编码
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns>编码后的参数</returns>
        public static string EncodeParms(string parms)
        {
            return System.Web.HttpUtility.UrlEncode(parms);
        }

        /// <summary>
        /// 对URL参数解码
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns>解码后的参数</returns>
        public static string DecodeParms(string parms)
        {
            return System.Web.HttpUtility.UrlDecode(parms);
        }
    }
}
