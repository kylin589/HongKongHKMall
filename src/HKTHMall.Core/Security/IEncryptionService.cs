
using BrCms.Core.Infrastructure;

namespace HKTHMall.Core.Security 
{
    public interface IEncryptionService : IDependency
    {
        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");

         //<summary>
         //RSA加密
         //</summary>
         //<param name="content">需要加密字符</param>
         //<returns></returns>
        string RSAEncrypt(string content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="content">需要解密字符</param>
        /// <returns></returns>
        string RSADecrypt(string content);
    }
}
