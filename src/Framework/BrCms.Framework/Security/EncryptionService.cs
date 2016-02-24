using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BrCms.Framework.Configuration;

namespace BrCms.Framework.Security
{
    public class EncryptionService : IEncryptionService
    {
        private readonly SecuritySettings _securitySettings;
        public EncryptionService(SecuritySettings securitySettings)
        {
            this._securitySettings = securitySettings;
        }

        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public virtual string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name", "saltkey");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = this._securitySettings.EncryptionKey;

            var tDESalg = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            byte[] encryptedBinary = this.EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);
            return Convert.ToBase64String(encryptedBinary);
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = this._securitySettings.EncryptionKey;

            var tDESalg = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            byte[] buffer = Convert.FromBase64String(cipherText);
            return this.DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
        }

        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = new UnicodeEncoding().GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    var sr = new StreamReader(cs, new UnicodeEncoding());
                    return sr.ReadLine();
                }
            }
        }

        #endregion

        #region RSA

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey">公私</param>
        /// <param name="content">明文</param>
        /// <returns>密文</returns>
        public string RsaEncrypt(RSAParameters publickey, string content)
        {

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publickey);
                var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                return Convert.ToBase64String(cipherbytes);
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="content">明文</param>
        /// <returns>密文</returns>
        public string RsaEncrypt(string content)
        {
            return this.RsaEncrypt(GetKey(false), content);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">私匙</param>
        /// <param name="content">密文</param>
        /// <returns>明文</returns>
        public string RsaDecrypt(RSAParameters privatekey, string content)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privatekey);
                var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content">密文</param>
        /// <returns>明文</returns>
        public string RsaDecrypt(string content)
        {
            return RsaDecrypt(GetKey(true), content);
        }

        /// <summary>
        /// 公私对象
        /// </summary>
        private static RSAParameters GetKey(bool includePrivateParameters)
        {
            // Create the CspParameters object and set the key container  
            // name used to store the RSA key pair. 
            CspParameters cp = new CspParameters
            {
                KeyContainerName = SiteConfig.Configuration.ContainerName,
                KeyNumber = 1, //设置密钥类型为Exchange
                Flags = CspProviderFlags.UseMachineKeyStore //设置密钥容器保存到计算机密钥库（默认为用户密钥库）
            };

            // Create a new instance of RSACryptoServiceProvider that accesses 
            // the key container MyKeyContainerName. 
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp))
            {
                return rsa.ExportParameters(includePrivateParameters);
            }
        }

        #endregion
    }
}
