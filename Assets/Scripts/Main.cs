﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using UnityEngine;

namespace SoldakModdingTool
{
    public static class Paths
    {
        //public static string FilesToEditPath = Application.persistentDataPath + "/FilesToEdit";
        public static string OutputPath = Application.persistentDataPath + "/OutPut";
    }

    public class Main : MonoBehaviour
    {
        public static List<ToolButton> Buttons = new List<ToolButton>()
        {
            new ToolSpellsUseWeaponDmg(),
            new ToolOverrideNameSetter(),
        };

        public void Start()
        {
            Debug.Log("Starting Program");

            // foreach (string filetxt in GetAllFileTxtInFolder(path)) {
            //    GetObjectsFromDBRFile(filetxt);
            // }
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
            //Debug.Log("file before removing comments: " + file);
            file = RemoveComments(file);
            //Debug.Log("file after removing comments: " + file);

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