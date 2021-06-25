using PassKeeper.Model;
using PassKeeper.Security;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PassKeeper.FileOperator
{
    public static class DbFile
    {
        private static readonly int[] keys = { 10, 20, 30, 40, 50, 60, 70, 80 };
        public  static readonly string fileExtension = ".dbxx";
        public static string Save(DbConfig dbConfig, string path)
        {
            return Save(dbConfig, null, path);
        }
        public static string Save(DbConfig dbConfig, List<ItemData> itemDatas, string dbFileName)
        {
            if (dbFileName.Substring(dbFileName.Length-5,5).ToLower() != fileExtension)
                dbFileName += fileExtension;
            dbConfig.EncrptKey = PasswordGenerator.GeneratePassword(8, true, true, true, false);
            string strDbConfig = JsonSerializer.Serialize(dbConfig);
            string keyData = PasswordGenerator.GeneratePassword(strDbConfig.Length, true, false, true, false).Encrypt(dbConfig.EncrptKey);
            string encryptKey = GetEncryptKey(keyData);
            List<string> lines = new();
            lines.Add(strDbConfig.Encrypt(encryptKey));
            if (itemDatas != null)
            {
                foreach (ItemData itemData in itemDatas)
                {
                    lines.Add(JsonSerializer.Serialize(itemData).Encrypt(dbConfig.EncrptKey));
                }
            }
            lines.Add(keyData);
            WriteFile(dbFileName, lines.ToArray());
            return dbFileName;
        }
        public static DbConfig ReadDbCofnig(string dbFilename)
        {
            string[] lines = ReadFile(dbFilename);
            if (lines.Length > 1)
            {
                string encryptKey = GetEncryptKey(lines[^1]);
                return JsonSerializer.Deserialize<DbConfig>(lines[0].Decrypt(encryptKey));
            }
            else
                return null;
        }
        public static List<ItemData> ReadItemDatas(DbConfig dbConfig, string dbFilename)
        {
            List<ItemData> itemDatas = new();
            string[] lines = ReadFile(dbFilename);
            if (lines.Length > 2)
            {
                for (int i = 1; i < lines.Length - 1; i++)
                {
                    itemDatas.Add(JsonSerializer.Deserialize<ItemData>(lines[i].Decrypt(dbConfig.EncrptKey)));
                }
                return itemDatas;
            }
            else
                return null;
        }
        private static void WriteFile(string fileName, string[] lines)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
            File.WriteAllLines(fileName, lines);
        }
        private static string[] ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
                return null;
            return File.ReadAllLines(fileName);
        }
        private static string GetEncryptKey(string keyData)
        {
            string encryptKey = "";
            foreach (int key in keys)
            {
                encryptKey += keyData[key];
            }
            return encryptKey;
        }
    }
}
