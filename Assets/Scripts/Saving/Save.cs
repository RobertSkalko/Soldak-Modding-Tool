using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SoldakModdingTool
{
    public class EditorData
    {
        private string nameContains = "";
        private string anyPartContains = "";
        private string isDerivedOf = "";

        public string NameContains { get => nameContains; set { nameContains = value; EditorGenerator.OnFilterUpdated?.Invoke(); } }
        public string AnyPartContains { get => anyPartContains; set { anyPartContains = value; EditorGenerator.OnFilterUpdated?.Invoke(); } }
        public string IsDerivedOf { get => isDerivedOf; set { isDerivedOf = value; EditorGenerator.OnFilterUpdated?.Invoke(); } }
    }

    public class Save
    {
        private static Save file = null;

        public static Save File {
            get {
                if (file == null) {
                    TryLoadStateFromFile();
                }

                return file;
            }

            set => file = value;
        }

        private static string SaveFileName = "Save.txt";
        private static string SaveDataPath = Application.persistentDataPath + "/SavedData/" + SaveFileName;
        private static string SaveDataPathWithoutFileName = Application.persistentDataPath + "/SavedData/";

        public EditorData EditorDatas = new EditorData();

        public string InputCommand;
        public string OutputPath;
        public string GamePath;
        public string FilesToEditPath;
        public string ModName;

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
            if (System.IO.File.Exists(SaveDataPath)) {
                file = JsonConvert.DeserializeObject<Save>(System.IO.File.ReadAllText(SaveDataPath), deSettings) ?? new Save();
            }
            else {
                file = new Save();
            }
        }

        public static void SaveStateToFile()
        {
            string json = JsonConvert.SerializeObject(File, Formatting.Indented, serSettings);

            if (!Directory.Exists(SaveDataPathWithoutFileName)) {
                Directory.CreateDirectory(SaveDataPathWithoutFileName);
            }

            if (!System.IO.File.Exists(SaveDataPath)) {
                System.IO.File.Create(SaveFileName);
            }

            System.IO.File.WriteAllText(SaveDataPath, json);
        }
    }
}