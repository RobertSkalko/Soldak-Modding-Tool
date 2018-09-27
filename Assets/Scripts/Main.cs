using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;
using UnityEngine;

namespace SoldakModdingTool
{
    public class Main : MonoBehaviour
    {
        public static List<ToolButton> Buttons => GetAllButtons();

        private static List<ToolButton> GetAllButtons()
        {
            List<Type> derivedTypes = VType.GetDerivedTypes(typeof(ToolButton), Assembly.GetExecutingAssembly());

            List<ToolButton> buttons = new List<ToolButton>();

            derivedTypes.ForEach(x => buttons.Add((ToolButton)Activator.CreateInstance(x)));

            return buttons;
        }

        public void Start()
        {
            Debug.Log("Starting Program");
        }

        public static void SaveOutputToFile(string file)
        {
            if (!Directory.Exists(Save.file.OutputPath)) {
                Debug.Log("OutputPath Doesn't exist or not Inputed!");
                return;
            }
            string filename = Save.file.OutputPath + "/" + DateTime.Now.ToFileTime() + ".txt";

            var bytes = Encoding.ASCII.GetBytes(file);

            File.Create(filename).Write(bytes, 0, bytes.Length);

            Debug.Log("File saved to \"" + Save.file.OutputPath + "\"");
        }

        public static void WriteChangedFiles(string path, List<string> files)
        {
            foreach (string file in files) {
                File.WriteAllText(path, file);
            }
        }

        public static List<SoldakObject> GetObjectsFromAllFilesInPath(string path)
        {
            List<SoldakObject> list = new List<SoldakObject>();

            foreach (string file in GetAllFileTxtInFolder(path)) {
                list.AddRange(GetObjectsFromDBRFile(file));
            }
            return list;
        }

        public static List<string> GetAllFileTxtInFolder(string path)
        {
            List<string> filetxts = new List<string>();

            foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories)) {
                if (file.EndsWith(".zip") || file.EndsWith(".Zip")) {
                    foreach (var zippedFile in ZipFile.Read(file).Entries) {
                        if (zippedFile.FileName.EndsWith(".gdb")) {
                            filetxts.Add(new StreamReader(zippedFile.OpenReader()).ReadToEnd());
                        }
                    }
                }
                else if (file.EndsWith(".gdb")) {
                    filetxts.Add(File.ReadAllText(file));
                }
            }
            return filetxts;
        }

        public static string RemoveComments(string file)
        {
            Regex comments1 = new Regex(@"//.*?\n");
            Regex comments2 = new Regex(@"/\*(.|\n)*?\*/");

            string afterRemoval = comments1.Replace(file, "");
            afterRemoval = comments2.Replace(afterRemoval, "");

            return afterRemoval;
        }

        public static List<SoldakObject> GetObjectsFromDBRFile(string file)
        {
            file = RemoveComments(file);

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