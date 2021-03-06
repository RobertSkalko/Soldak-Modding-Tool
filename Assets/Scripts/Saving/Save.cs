﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SoldakModdingTool
{
    public class EditorData
    {
        [JsonRequired] private string nameContains = "";
        [JsonRequired] private string anyPartContains = "";
        [JsonRequired] private string isDerivedOf = "";

        [JsonIgnore] public string NameContains { get => nameContains; set { nameContains = value; EditorGenerator.OnFilterUpdated?.Invoke(); } }
        [JsonIgnore] public string NameDoesntHave { get => anyPartContains; set { anyPartContains = value; EditorGenerator.OnFilterUpdated?.Invoke(); } }
        [JsonIgnore] public string IsDerivedOf { get => isDerivedOf; set { isDerivedOf = value; EditorGenerator.OnFilterUpdated?.Invoke(); } }
    }

    public class Save
    {
        private static Save instance = null;

        public static Save Instance {
            get {
                if (instance == null) {
                    TryLoadStateFromFile();
                }

                return instance;
            }

            set => instance = value;
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
            string savepath = SaveDataPath;

            if (File.Exists(savepath)) {
                instance = (JsonConvert.DeserializeObject<Save>(File.ReadAllText(savepath), deSettings) != null) ? JsonConvert.DeserializeObject<Save>(File.ReadAllText(savepath), deSettings) : new Save();
            }
            else {
                instance = new Save();
            }
        }

        public static void SaveStateToFile()
        {
            string json = JsonConvert.SerializeObject(Instance, Formatting.Indented, serSettings);

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