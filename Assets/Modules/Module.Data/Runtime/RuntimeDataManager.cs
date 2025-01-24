using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using EncosyTower.Modules;
using EncosyTower.Modules.Logging;
using EncosyTower.Modules.Serialization;
using EncosyTower.Modules.Vaults;
using UnityEngine;
using Module.Data.Runtime.Serialization;
using Sirenix.OdinInspector;

namespace Module.Data.Runtime
{
    public class RuntimeDataManager : MonoBehaviour
    {
        private const string FILE_NAME = "runtime-data";

        public static readonly Id<RuntimeDataManager> PresetId = default;

        private string _persistentDataPath;
        private RuntimeDataSerializeContainer _container;

        private void Awake()
        {
            _persistentDataPath = Application.persistentDataPath + "/";
            _container = new();

            GlobalObjectVault.TryAdd(PresetId, this);
            GlobalValueVault<bool>.TrySet(PresetId, true);
        }

        private void OnDestroy()
        {
            GlobalValueVault<bool>.TrySet(PresetId, false);
            GlobalObjectVault.TryRemove(PresetId, out _);
        }

        [Button(buttonSize:35)]
        public void Load()
        {
            _container = JsonDeserializeFromPathInternal(_persistentDataPath + FILE_NAME);

            DevLoggerAPI.LogInfo(_container.Entries.Length);
        }

        [Button(buttonSize: 35)]
        public void Save()
        {
            JsonSerializeToPathInternal(_container, _persistentDataPath + FILE_NAME);

            DevLoggerAPI.LogInfo(_container.Entries.Length);
        }

        [Button(buttonSize: 35)]
        public void TestAdd()
        {
            var data = new RuntimeDataSerialize();
            _container.AddEntry(data);
        }

        internal protected RuntimeDataSerializeContainer JsonDeserializeFromPathInternal(
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
                        JsonHelper.TryDeserialize<RuntimeDataSerializeContainer>(fileContents, out var data);
                        return data;
                    }
                    catch (Exception ex)
                    {
                        DevLoggerAPI.LogError(ex.Message);
                        return default(RuntimeDataSerializeContainer);
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
                return default(RuntimeDataSerializeContainer);
            }
        }

        internal protected void JsonSerializeToPathInternal(RuntimeDataSerializeContainer fileToSerialize, string absolutePath, string secureKey = "")
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

        internal protected void DeleteFileAtPathInternal(string absolutePath)
            => File.Delete(absolutePath);


        /// <summary>
        /// Checks if file exists at Persistent Data Path.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        /// <returns>True if file exists ans false otherwise.</returns>
        public bool FileExistsAtPath(string absolutePath)
        {
            return File.Exists(absolutePath);
        }

        #region    Encrypt 
        #endregion =======

        private string Encrypt(string clearText, string EncryptionKey)
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

        private string Decrypt(string cipherText, string EncryptionKey)
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

