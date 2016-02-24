using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BrCms.Framework.Logging;
using HKTHMall.Core.Configuration;

namespace HKTHMall.Core.Security
{
    public class EncryptionService : IEncryptionService
    {
        private readonly ILogger _logger;
        private readonly SecuritySettings _securitySettings;

        public EncryptionService()
        {
        }

        public EncryptionService(SecuritySettings securitySettings, ILogger logger)
        {
            this._securitySettings = securitySettings;
            this._logger = logger;
        }

        /// <summary>
        ///     Create salt key
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
        ///     Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (string.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            var saltAndPassword = string.Concat(password, saltkey);

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name", "saltkey");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        ///     Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = this._securitySettings.EncryptionKey;

            var tDESalg = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            var encryptedBinary = this.EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);
            return Convert.ToBase64String(encryptedBinary);
        }

        /// <summary>
        ///     Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = this._securitySettings.EncryptionKey;

            var tDESalg = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            var buffer = Convert.FromBase64String(cipherText);
            return this.DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
        }

        /// <summary>
        ///     RSA加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string RSAEncrypt(string content)
        {
            try
            {
                var publickey =
                    @"<RSAKeyValue><Modulus>wVwBKuePO3ZZbZ//gqaNuUNyaPHbS3e2v5iDHMFRfYHS/bFw+79GwNUiJ+wXgpA7SSBRhKdLhTuxMvCn1aZNlXaMXIOPG1AouUMMfr6kEpFf/V0wLv6NCHGvBUK0l7O+2fxn3bR1SkHM1jWvLPMzSMBZLCOBPRRZ5FjHAy8d378=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                var rsa = new RSACryptoServiceProvider();
                byte[] cipherbytes;
                rsa.FromXmlString(publickey);
                cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

                return Convert.ToBase64String(cipherbytes);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
       
        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string RSADecrypt(string content)
        {
            if (null == content || "0".Equals(content))
            {
                return content;
            }
            var sourceContent = content;
            try
            {
                content = content.Replace(" ", "+").Replace("\\", "");
                sourceContent = content;
                var privatekey =
                    @"<RSAKeyValue><Modulus>wVwBKuePO3ZZbZ//gqaNuUNyaPHbS3e2v5iDHMFRfYHS/bFw+79GwNUiJ+wXgpA7SSBRhKdLhTuxMvCn1aZNlXaMXIOPG1AouUMMfr6kEpFf/V0wLv6NCHGvBUK0l7O+2fxn3bR1SkHM1jWvLPMzSMBZLCOBPRRZ5FjHAy8d378=</Modulus><Exponent>AQAB</Exponent><P>64ZxmWRaS8jXsVhv1IOQh+4dD9z9jfa9BAWDPvQykHcLUKE1h1jGoOTf6xby+4Wmb9FXdXifNj1WnJAwD1LGfw==</P><Q>0isr6Q0S01fL9HkOdrf5EJRIehhl4KZtFwEnEreNCg7PnDUlwVM9Uw+bGKrCzy0ZT1pbry9DkWLPY0srK9DGwQ==</Q><DP>DKoaCal/wXt3Pa4HtWGtr+F55pR3fd66ozC4sfXnkiUUkq1Yd4Kqi5RDBh0hy6yQGosjLMnjpcL+mUSXkPteeQ==</DP><DQ>g4/U1/mAHF5sZShWnoiB2BgK2qtlMuDbjzgAfp36Ix6sZat7a+6wh8tQGnvioRApNNxqYlqi4GLLUevfJXl2wQ==</DQ><InverseQ>kDJPNy+K90v4dAwUbREsx8fJAy3k0QAEy5Jk+Mq0ZIVzfTZ6tX4W+J1N8VwpM0uZcV+1nZiLu4E3ePaZgZQWig==</InverseQ><D>B3Dc8qO6lVU2l8tib8qtBYYc7wDvqXXP6Iub8A1Yb3YBgpXDfUydEmqhR9wEA5g9T9EYkfxGIbhsV0N/ke82aQriEBug4sUsRHiqfpfyW+MH1AHi71Z4qpu3GtjPuFEwKlCVDunK8xOn0cqYEs/SMnODJnbYMmtlcnfFic8PwQE=</D></RSAKeyValue>";
                var rsa = new RSACryptoServiceProvider();
                byte[] cipherbytes;
                rsa.FromXmlString(privatekey);
                cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
            catch (Exception ex)
            {
                this._logger.Error(this.GetType().FullName,
                    string.Format("类型名称:{0},方法名称:{1},错误信息:{2}", this.GetType().FullName, "RSADecrypt", ex.Message));
                return string.Empty;
            }
        }

        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (
                    var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
                        CryptoStreamMode.Write))
                {
                    var toEncrypt = new UnicodeEncoding().GetBytes(data);
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
                using (
                    var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv),
                        CryptoStreamMode.Read))
                {
                    var sr = new StreamReader(cs, new UnicodeEncoding());
                    return sr.ReadLine();
                }
            }
        }

        #endregion

        #region RSA

        /// <summary>
        ///     RSA加密
        /// </summary>
        /// <param name="publickey">公私</param>
        /// <param name="content">明文</param>
        /// <returns>密文</returns>
        public string RsaEncrypt(RSAParameters publickey, string content)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publickey);
                var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                return Convert.ToBase64String(cipherbytes);
            }
        }

        /// <summary>
        ///     RSA加密
        /// </summary>
        /// <param name="content">明文</param>
        /// <returns>密文</returns>
        public string RsaEncrypt(string content)
        {
            return this.RsaEncrypt(GetKey(false), content);
        }

        /// <summary>
        ///     RSA解密
        /// </summary>
        /// <param name="privatekey">私匙</param>
        /// <param name="content">密文</param>
        /// <returns>明文</returns>
        public string RsaDecrypt(RSAParameters privatekey, string content)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privatekey);
                var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
        }

        /// <summary>
        ///     RSA解密
        /// </summary>
        /// <param name="content">密文</param>
        /// <returns>明文</returns>
        public string RsaDecrypt(string content)
        {
            return this.RsaDecrypt(GetKey(true), content);
        }

        /// <summary>
        ///     公私对象
        /// </summary>
        private static RSAParameters GetKey(bool includePrivateParameters)
        {
            // Create the CspParameters object and set the key container  
            // name used to store the RSA key pair. 
            var cp = new CspParameters
            {
                KeyContainerName = SiteConfig.Configuration.ContainerName,
                KeyNumber = 1, //设置密钥类型为Exchange
                Flags = CspProviderFlags.UseMachineKeyStore //设置密钥容器保存到计算机密钥库（默认为用户密钥库）
            };

            // Create a new instance of RSACryptoServiceProvider that accesses 
            // the key container MyKeyContainerName. 
            using (var rsa = new RSACryptoServiceProvider(cp))
            {
                return rsa.ExportParameters(includePrivateParameters);
            }
        }

        #endregion

         
    }
}