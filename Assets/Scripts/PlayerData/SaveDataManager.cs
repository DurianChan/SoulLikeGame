using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace DC {
    ///<summary>
    ///数据存储管理
    ///</summary>
    public static class SaveDataManager
    {
        public static string directoryName = "SaveData";            //存储数据文件夹

        /// <summary>
        /// 检测保存文件是否存在
        /// </summary>
        /// <returns></returns>
        private static bool SaveExists(string fileName)
        {
            return File.Exists(GetFullPath(fileName));
        }

        /// <summary>
        /// 检测数据文件夹是否存在
        /// </summary>
        /// <returns></returns>
        private static bool DirectoryExists()
        {
            return Directory.Exists(Application.persistentDataPath + "/" + directoryName);
        }

        /// <summary>
        /// 获取完成数据文件路径
        /// </summary>
        /// <returns></returns>
        private static string GetFullPath(string fileName)
        {
            return Application.persistentDataPath + "/" + directoryName + "/" + fileName;
        }

        /// <summary>
        /// 保存：写入数据
        /// </summary>
        /// <param name="so"></param>
        public static void SaveFile<T>(string fileName,T saveObject)
        {
            if (!DirectoryExists())
                Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetFullPath(fileName));
            bf.Serialize(file, saveObject);
            file.Close();
        }

        /// <summary>
        /// 载入：获取数据
        /// </summary>
        public static T LoadFile<T>(string fileName)
        {
            if (SaveExists(fileName))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(GetFullPath(fileName), FileMode.Open);
                    T loadObject = (T)bf.Deserialize(file);
                    file.Close();

                    return loadObject;
                }
                catch (SerializationException)
                {
                    Debug.Log("载入文件失败!");
                    File.Delete(GetFullPath(fileName));
                }
            }
            return default(T);
        } 
    }
}
