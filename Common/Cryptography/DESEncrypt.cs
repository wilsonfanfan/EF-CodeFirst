using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace X.Common
{
    /// <summary>
    /// 加密编码类
    /// </summary>
    public class DESEncrypt
    {
        /// <summary>
        /// 默认KEY
        /// </summary>
        public const string DefaultKey = "x_defaultkey";
        #region 生成随机安全码
        /// <summary>
        /// 生成随机安全码.
        /// </summary>
        public static string GenerateKey(int numBytes)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[numBytes / 2];
            rng.GetBytes(buff);
            StringBuilder hexString = new StringBuilder(numBytes);
            for (int counter = 0; counter < buff.Length; counter++)
            {
                hexString.Append(String.Format("{0:X2}", buff[counter]));
            }
            return hexString.ToString();
        }
        #endregion

        #region 非对称加密 MD5 SHA
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="star_index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input, int star_index = 0, int length = 32)
        {
            byte[] data = GetByte(input);
            data = MakeMD5(data);
            return ByteToString(data).Substring(star_index, length);
        }
        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns></returns>
        public static byte[] MakeMD5(byte[] original)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(original);
                return data;
            }
        }
        /// <summary>
        /// SHA函数
        /// </summary>
        ///  <param name="input">原始字符串</param>
        /// <returns></returns>
        public static string SHA(string input)
        {
            using (SHA1 sha = SHA1.Create())
            {
                byte[] data = GetByte(input);
                data = sha.ComputeHash(data);
                return ByteToString(data);
            }
        }
        /// <summary>
        /// SHA256函数
        /// </summary>
        ///  <param name="input">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string input)
        {
            using (SHA256Managed sha = new SHA256Managed())
            {
                byte[] data = GetByte(input);
                data = sha.ComputeHash(data);
                return ByteToString(data);
            }
        }
        #endregion

        #region 对称加密解密
        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            byte[] data = GetByte(plainText);
            return Convert.ToBase64String(data);
        }
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string encrypted)
        {
            byte[] outputb = Convert.FromBase64String(encrypted);
            return GetString(outputb);
        }
        #endregion

        #region 密码
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="input">密码</param>
        /// <param name="RndKey">随机密钥</param>
        public static string EncryptPwd(string input, string RndKey)
        {
            return Base64Encode(DESEncrypt.SHA256(DESEncrypt.Encrypt(input, RndKey)));
        }

        /// <summary>
        /// 随机密钥
        /// </summary>
        public static string GetRndKey()
        {
            return GetMd5Hash(System.Guid.NewGuid().ToString());
        }
        #endregion

        #region  使用 给定密钥 加密/解密
        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key = DefaultKey)
        {
            var o = GetByte(original);
            var k = GetByte(key);
            var encrypted = EncryptByte(o, k);
            return ByteToString(encrypted);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key = DefaultKey)
        {
            try
            {
                var o = StringToByte(encrypted);
                var k = GetByte(key);
                var original = DecryptByte(o, k);
                return GetString(original);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string TripleEncrypt(string original, string key = DefaultKey)
        {
            var o = GetByte(original);
            var k = GetByte(key);
            var encrypted = TripleEncryptByte(o, k);
            return ByteToString(encrypted);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string TripleDecrypt(string encrypted, string key = DefaultKey)
        {
            try
            {
                var o = StringToByte(encrypted);
                var k = GetByte(key);
                var original = TripleDecryptByte(o, k);
                return GetString(original);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region  使用 给定密钥 加密/解密/byte[]
        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] EncryptByte(byte[] original, byte[] key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                var data = MakeMD5(key).Take(8).ToArray();
                des.Key = data;
                des.IV = data;
                return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
            }
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] DecryptByte(byte[] encrypted, byte[] key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                var data = MakeMD5(key).Take(8).ToArray();
                des.Key = data;
                des.IV = data;
                return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
        }
        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] TripleEncryptByte(byte[] original, byte[] key)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                var data = MakeMD5(key);
                des.Key = data;
                des.IV = data.Take(8).ToArray();
                return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
            }
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] TripleDecryptByte(byte[] encrypted, byte[] key)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                var data = MakeMD5(key);
                des.Key = data;
                des.IV = data.Take(8).ToArray();
                return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
        }

        #endregion

        #region 私有
        private static Encoding Enc = Encoding.UTF8;
        private static string ByteToString(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte b in data)
            {
                sBuilder.AppendFormat("{0:X2}", b);
            }
            return sBuilder.ToString();
        }
        private static byte[] StringToByte(string input)
        {
            int len;
            len = input.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(input.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            return inputByteArray;
        }
        private static byte[] GetByte(string input)
        {
            return Enc.GetBytes(input);
        }
        private static string GetString(byte[] data)
        {
            return Enc.GetString(data);
        }
        #endregion
    }
}
