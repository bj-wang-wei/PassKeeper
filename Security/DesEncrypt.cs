using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PassKeeper.Security
{
    public static class DesEncrypt
    {
        /// <summary> 
        /// 加密字符串
        /// 加密密钥必须为8位
        /// </summary> 
        /// <param name="strText">被加密的字符串</param> 
        /// <param name="strEncryptKey">8位长度密钥</param> 
        /// <returns>加密后的数据</returns> 
        public static string Encrypt(this string strText, string strEncryptKey)
        {
            if (strEncryptKey.Length < 8)
            {
                throw new Exception("密钥长度无效，密钥必须是8位！");
            }

            StringBuilder ret = new();
            using var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(strText);
            des.Key = Encoding.ASCII.GetBytes(strEncryptKey.Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(strEncryptKey.Substring(0, 8));
            MemoryStream ms = new();
            using var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat($"{b:X2}");
            }

            return ret.ToString();
        }
        /// <summary>
        ///     DES解密算法
        ///     密钥为8位
        /// </summary>
        /// <param name="pToDecrypt">需要解密的字符串</param>
        /// <param name="strEncryptKey">密钥</param>
        /// <returns>解密后的数据</returns>
        public static string Decrypt(this string pToDecrypt, string strEncryptKey)
        {
            if (strEncryptKey.Length < 8)
            {
                throw new Exception("密钥长度无效，密钥必须是8位！");
            }

            var ms = new MemoryStream();
            using var des = new DESCryptoServiceProvider();
            var inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(strEncryptKey.Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(strEncryptKey.Substring(0, 8));
            using var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}
