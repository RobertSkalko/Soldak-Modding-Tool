using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SoldakModdingTool
{
    public class Save
    {
        public static Save file = new Save();

        private static string SaveFileName = "Save.txt";
        private static string SaveDataPath = Application.persistentDataPath + "/SavedData/" + SaveFileName;
        private static string SaveDataPathWithoutFileName = Application.persistentDataPath + "/SavedData/";

        public string GamePath;
        public string FilesToEditPath;

        private static JsonSerializerSettings serSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        };

        private static JsonSerializerSettings deSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
        };

        public static void TryLoadStateFromFile()
        {
            if (File.Exists(SaveDataPath)) {
                file = JsonConvert.DeserializeObject<Save>(File.ReadAllText(SaveDataPath), deSettings) ?? new Save();
            }
            else {
                file = new Save();
            }
        }

        public static void SaveStateToFile()
        {
            string json = JsonConvert.SerializeObject(file, Formatting.Indented, serSettings);

            if (!Directory.Exists(SaveDataPathWithoutFileName)) {
                Directory.CreateDirectory(SaveDataPathWithoutFileName);
            }

            if (!File.Exists(SaveDataPath)) {
                File.Create(SaveFileName);
            }

            File.WriteAllText(SaveDataPath, json);
        }
    }
}