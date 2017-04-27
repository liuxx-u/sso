using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace sso.Utils
{
    /// <summary>
    /// DES / TripleDES加密解密操作类
    /// </summary>
    public class DesHelper
    {
        private readonly bool _isTriple;
        private readonly byte[] _key;

        /// <summary>
        /// 使用随机密码初始化一个<see cref="DesHelper"/>类的新实例
        /// </summary>
        /// <param name="isTriple">是否使用TripleDES方式，否则为DES方式</param>
        public DesHelper(bool isTriple = false)
            : this(isTriple
                ? new TripleDESCryptoServiceProvider().Key
                : new DESCryptoServiceProvider().Key)
        {
            _isTriple = isTriple;
        }

        /// <summary>
        /// 使用指定8位或24位密码初始化一个<see cref="DesHelper"/>类的新实例
        /// </summary>
        public DesHelper(byte[] key)
        {
            key.CheckNotNull("key");
            key.Required(k => k.Length == 8 || k.Length == 24, $"参数key的长度必须为8或24，当前为{key.Length}。");
            _key = key;
            _isTriple = key.Length == 24;
        }

        /// <summary>
        /// 获取 密钥
        /// </summary>
        public byte[] Key => _key;

        #region 实例方法

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="source">要加密的字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] source)
        {
            source.CheckNotNull("source");
            SymmetricAlgorithm provider = _isTriple
                ? (SymmetricAlgorithm)new TripleDESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB }
                : new DESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB };

            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(source, 0, source.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="source">要解密的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] source)
        {
            source.CheckNotNull("source");

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            SymmetricAlgorithm provider = _isTriple
                ? (SymmetricAlgorithm)new TripleDESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB }
                : new DESCryptoServiceProvider { Key = _key, Mode = CipherMode.ECB };
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(source, 0, source.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 加密字符串，输出BASE64编码字符串
        /// </summary>
        /// <param name="source">要加密的明文字符串</param>
        /// <returns>加密的BASE64编码的字符串</returns>
        public string Encrypt(string source)
        {
            source.CheckNotNull("source");
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return Base64UrlEncode(Encrypt(bytes));
        }

        /// <summary>
        /// 解密字符串，输入为BASE64编码字符串
        /// </summary>
        /// <param name="source">要解密的BASE64编码的字符串</param>
        /// <returns>明文字符串</returns>
        public string Decrypt(string source)
        {
            source.CheckNotNullOrEmpty("source");
            byte[] bytes = Base64UrlDecode(source);
            return Encoding.UTF8.GetString(Decrypt(bytes));
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="source">要加密的字节数组</param>
        /// <param name="key">密钥字节数组，长度为8或者24</param>
        /// <returns>加密后的字节数组</returns>
        public static byte[] Encrypt(byte[] source, byte[] key)
        {
            DesHelper des = new DesHelper(key);
            return des.Encrypt(source);
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="source">要解密的字节数组</param>
        /// <param name="key">密钥字节数组，长度为8或者24</param>
        /// <returns>解密后的字节数组</returns>
        public static byte[] Decrypt(byte[] source, byte[] key)
        {
            DesHelper des = new DesHelper(key);
            return des.Decrypt(source);
        }

        /// <summary>
        /// 加密字符串，输出BASE64编码字符串
        /// </summary>
        /// <param name="source">要加密的明文字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns>加密的BASE64编码的字符串</returns>
        public static string Encrypt(string source, string key)
        {
            source.CheckNotNull("source");
            key.CheckNotNullOrEmpty("key");
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            return des.Encrypt(source);
        }

        /// <summary>
        /// 解密字符串，输入BASE64编码字符串
        /// </summary>
        /// <param name="source">要解密的BASE64编码字符串</param>
        /// <param name="key">密钥字符串，长度为8或者24</param>
        /// <returns>解密的明文字符串</returns>
        public static string Decrypt(string source, string key)
        {
            source.CheckNotNullOrEmpty("source");
            key.CheckNotNullOrEmpty("key");
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            DesHelper des = new DesHelper(keyBytes);
            return des.Decrypt(source);
        }

        #endregion

        private string Base64UrlEncode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private byte[] Base64UrlDecode(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            return Convert.FromBase64String(Pad(text.Replace('-', '+').Replace('_', '/')));
        }

        private string Pad(string text)
        {
            int num = 3 - (text.Length + 3) % 4;
            if (num == 0)
            {
                return text;
            }
            return text + new string('=', num);
        }
    }
}
