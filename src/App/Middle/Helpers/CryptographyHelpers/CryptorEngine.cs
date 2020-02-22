using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Globalization;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Middle.Helpers.CryptographyHelpers
{
    public class CryptorEngine
    {
        const string delimited = "|";
        private static string _key = string.Empty;

        public static string Key
        {
            get
            {
                if (String.IsNullOrEmpty(_key))
                    _key = RegistryHelpers.RegistryHelpers.ReturnKey(@"SOFTWARE\eFolding", "Key").ToString();

                return _key;
            }
            //set { CryptorEngine.key = value; }
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            return Encrypt(toEncrypt, useHashing, Key);
        }

        public static string Encrypt(int toEncrypt, bool useHashing, string customKey)
        {
            return Encrypt(toEncrypt.ToString(), useHashing, customKey);
        }

        public static object Encrypt(object toEncrypt, bool useHashing, string customKey)
        {
            return Encrypt(toEncrypt.ToString(), useHashing, customKey);
        }

        public static string Encrypt(string toEncrypt, bool useHashing, string customKey)
        {
            MD5CryptoServiceProvider hashmd5 = null;
            TripleDESCryptoServiceProvider tdes = null;
            ICryptoTransform cTransform = null;

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(customKey));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(Key);

            tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            if (hashmd5 != null)
                hashmd5.Dispose();

            if (tdes != null)
                tdes.Dispose();

            if (cTransform != null)
                cTransform.Dispose();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length) + delimited;
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipher, bool useHashing)
        {
            return Decrypt(cipher, useHashing, Key);
        }

        public static string Decrypt(string cipher, bool useHashing, string customKey)
        {
            try
            {
                string ret = string.Empty;

                foreach (string value in cipher.Split(delimited[0]))
                {
                    if (!String.IsNullOrEmpty(value)) // != string.Empty)
                        ret += SubDecrypt(value, useHashing, customKey);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string SubDecrypt(string cipherString, bool useHashing, string customKey)
        {
            MD5CryptoServiceProvider hashmd5 = null;
            TripleDESCryptoServiceProvider tdes = null;
            ICryptoTransform cTransform = null;

            try
            {
                byte[] keyArray;
                byte[] toEncryptArray = toEncryptArray = Convert.FromBase64String(cipherString);

                if (useHashing)
                {
                    hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(customKey));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(Key);

                tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (hashmd5 != null)
                {
                    hashmd5.Dispose();
                }

                if (tdes != null)
                {
                    tdes.Dispose();
                }
                if (cTransform != null)
                {
                    cTransform.Dispose();
                }
            }
        }
    }
}
