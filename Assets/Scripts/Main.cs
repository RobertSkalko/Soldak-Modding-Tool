using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;
using UnityEngine;

namespace SoldakModdingTool
{
    public class Main : MonoBehaviour
    {
        public static List<ToolButton> Buttons = new List<ToolButton>()
        {
            new ToolSpellsUseWeaponDmg(),
            new ToolValidityChecker(),
        };

        public void Start()
        {
            Debug.Log("Starting Program");

            string path = Application.persistentDataPath + "/FilesToEdit";

            // foreach (string filetxt in GetAllFileTxtInFolder(path)) {
            //    GetObjectsFromDBRFile(filetxt);
            // }
        }

        public static void WriteChangedFiles(string path, List<string> files)
        {
            foreach (string file in files) {
                File.WriteAllText(path, file);
            }
        }

        public static List<string> GetAllFileTxtInFolder(string path)
        {
            List<string> filetxts = new List<string>();

            foreach (string file in Directory.GetFiles(path, "*.zip", SearchOption.AllDirectories)) {
                foreach (var zippedFile in ZipFile.Read(file).Entries) {
                    if (zippedFile.FileName.EndsWith(".gdb")) {
                        filetxts.Add(new StreamReader(zippedFile.OpenReader()).ReadToEnd());
                    }
                }
            }
            return filetxts;
        }

        public static string RemoveComments(string file)
        {
            List<string> lines = file.Split('\n').ToList();

            for (var i = 0; i < lines.Count; i++) {
                string s = lines[i];
                if (s.Contains("//")) {
                    lines[i] = s.Remove(s.IndexOf("//"));
                }
            }

            string final = "";

            lines.ForEach(x => final += x + "\n");

            return final;
        }

        public static List<SoldakObject> GetObjectsFromDBRFile(string file)
        {
            Debug.Log("file before removing comments: " + file);
            file = RemoveComments(file);
            Debug.Log("file after removing comments: " + file);

            var objects = new List<SoldakObject>();

            while (file.Contains("}")) {
                int end = file.IndexOf('}');

                objects.Add(new SoldakObject(file.Substring(0, end + 1)));

                file = file.Substring(end + 1);
            }

            return objects;
        }
    }

    public enum Modifiers
    {
        none,
        overrides,
        addsTo,
    }
}