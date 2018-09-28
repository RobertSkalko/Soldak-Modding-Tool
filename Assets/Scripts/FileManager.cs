using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ionic.Zip;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoldakModdingTool
{
    public static class FileManager
    {
        public static void SaveOutputToFile(string file)
        {
            if (!Directory.Exists(Save.file.OutputPath)) {
                Debug.Log("OutputPath Doesn't exist or not Inputed!");
                return;
            }
            string filename = Save.file.OutputPath + "/" + DateTime.Now.ToFileTime() + ".txt";

            var bytes = Encoding.ASCII.GetBytes(file);

            var stream = File.Create(filename);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            Debug.Log("File saved to \"" + Save.file.OutputPath + "\"");
        }

        public static void WriteChangedFiles(string path, ConcurrentBag<string> files)
        {
            foreach (string file in files) {
                File.WriteAllText(path, file);
            }
        }

        public static ConcurrentBag<SoldakObject> GetObjectsFromAllFilesInPath(string path, bool OnlyVanillaAssets = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var files = GetAllGDBFilesInFolder(path, OnlyVanillaAssets);

            var fileswithoutcomments = CommentRemover.RemoveCommentsFromList(files);

            var splitobjects = SplitObjectsFromFiles(fileswithoutcomments); // this takes half the time

            var soldakobjects = GenerateSoldakObjects(splitobjects); // also highly intensive

            stopwatch.Stop();
            Debug.Log("Getting files and generating objects took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return soldakobjects;
        }

        private static ConcurrentBag<string> SplitObjectsFromFiles(ConcurrentBag<string> Files)
        {
            var SplitObjects = new ConcurrentBag<string>();
            var bracket = "}";
            var anytext = @"^\S";

            Parallel.ForEach(Files, (file) => {
                var objs = file.Split(new string[] { anytext, bracket }, StringSplitOptions.RemoveEmptyEntries);

                SplitObjects.AddRange(objs.Where(x => !string.IsNullOrWhiteSpace(x)));
            });

            return SplitObjects;
        }

        private static ConcurrentBag<SoldakObject> GenerateSoldakObjects(ConcurrentBag<string> list)
        {
            var newlist = new ConcurrentBag<SoldakObject>();

            Parallel.ForEach(list, (file) => {
                newlist.Add(new SoldakObject(file));
            });

            return newlist;
        }

        private static ConcurrentBag<string> GetAllGDBFilesInsideZip(string file)
        {
            var list = new ConcurrentBag<string>();
            if (file.EndsWith(".zip")) {
                var entries = ZipFile.Read(file).AsParallel();
                foreach (var zippedFile in entries) {
                    if (zippedFile.FileName.EndsWith(".gdb")) {
                        var stream = new StreamReader(zippedFile.OpenReader());
                        var fileText = stream.ReadToEnd();
                        list.Add(fileText);
                        stream.Close();
                    }
                }
            }
            return list;
        }

        public static ConcurrentBag<string> GetAllGDBFilesInFolder(string path, bool OnlyVanillaAssets = false)
        {
            ConcurrentBag<string> filetxts = new ConcurrentBag<string>();

            Parallel.ForEach(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories), (file) => {
                if (OnlyVanillaAssets) {
                    if (file.Contains("assets")) {
                        filetxts.AddRange(GetAllGDBFilesInsideZip(file));
                    }
                }
                else {
                    filetxts.AddRange(GetAllGDBFilesInsideZip(file));
                    if (file.EndsWith(".gdb")) {
                        filetxts.Add(File.ReadAllText(file));
                    }
                }
            });

            return filetxts;
        }
    }
}