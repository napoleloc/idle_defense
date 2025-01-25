using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Module.Data.Runtime.Persistention
{
    internal static class PersistentHelper
    {
        private static readonly string s_persistentDataPath;

        internal static TData JsonDeserializeFromPathInternal<TData>(
            string absolutePath
            , string secureKey = ""
            , bool logIfFileNotExists = false
        )
        {
            if (FileExistsAtPath(absolutePath))
            {
                FileStream file = File.Open(absolutePath, FileMode.Open);

                using (StreamReader reader = new StreamReader(file))
                {
                    string fileContents = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(secureKey) == false)
                        fileContents = Decrypt(fileContents, secureKey);

                    try
                    {
                        JsonHelper.TryDeserialize<TData>(fileContents, out var data);
                        return data;
                    }
                    catch (Exception ex)
                    {
                        DevLoggerAPI.LogError(ex.Message);
                        return default(TData);
                    }
                    finally
                    {
                        file.Close();
                    }
                }
            }
            else
            {
                if (logIfFileNotExists)
                {
                    DevLoggerAPI.LogError("File at path : \"" + absolutePath + "\" does not exist.");
                }
                return default(TData);
            }
        }

        internal static void JsonSerializeToPathInternal<TData>(TData fileToSerialize, string absolutePath, string secureKey = "")
        {
            if (JsonHelper.TrySerialize(fileToSerialize, out var json))
            {
                if (string.IsNullOrEmpty(secureKey) == false)
                    json = Encrypt(json, secureKey);

                FileStream stream = File.Open(absolutePath, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
                stream.Close();
            }
        }

        internal static void DeleteFileAtPathInternal(string absolutePath)
            => File.Delete(absolutePath);


        /// <summary>
        /// Checks if file exists at Persistent Data Path.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        /// <returns>True if file exists ans false otherwise.</returns>
        internal static bool FileExistsAtPath(string absolutePath)
        {
            return File.Exists(absolutePath);
        }

        #region    Encrypt 
        #endregion =======

        private static string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private static string Decrypt(string cipherText, string EncryptionKey)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
