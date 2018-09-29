﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Ionic.Zip;
using UnityEngine;
using UnityEngine.Profiling;
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
            //File.GetAccessControl(filename);
            File.WriteAllText(filename, file);

            Debug.Log("File saved to \"" + Save.file.OutputPath + "\"");
        }

        public static void WriteChangedFiles(string path, ConcurrentBag<string> files)
        {
            foreach (string file in files) {
                File.WriteAllText(path, file);
            }
        }

        public static ConcurrentBag<SoldakObject> GetObjectsFromAllFilesInPath(string path, bool OnlyVanillaAssets = false, bool AllowDuplicates = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Profiler.BeginSample("GetAllGDBFilesInFolder");

            var files = GetAllGDBFilesInFolder(path, OnlyVanillaAssets);
            Profiler.EndSample();
            Profiler.BeginSample("RemoveComments");

            var fileswithoutcomments = CommentRemover.RemoveComments(files);
            Profiler.EndSample();
            Profiler.BeginSample("SplitObjects");

            var splitobjects = SplitObjectsFromFiles(fileswithoutcomments); // this takes half the time
            Profiler.EndSample();
            Profiler.BeginSample("GenerateSoldakObjects");

            var soldakobjects = GenerateSoldakObjects(splitobjects, AllowDuplicates); // also highly intensive
            Profiler.EndSample();

            stopwatch.Stop();
            Debug.Log("Getting files and generating objects took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return soldakobjects;
        }

        private static ConcurrentDictionary<string, string> SplitObjectsFromFiles(ConcurrentDictionary<string, string> Files)
        {
            var SplitObjects = new ConcurrentDictionary<string, string>();
            var bracket = "}";
            var anytext = @"^\S";

            Parallel.ForEach(Files, (file) => {
                var objs = file.Key.Split(new string[] { anytext, bracket }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in file.Key.Split(new string[] { anytext, bracket }, StringSplitOptions.RemoveEmptyEntries)) {
                    if (!string.IsNullOrWhiteSpace(item)) {
                        SplitObjects.TryAdd(item, file.Key);
                    }
                }
            });

            return SplitObjects;
        }

        private static SoldakObject ReturnBasedOnFilePath(SoldakObject obj1, SoldakObject obj2)
        {
            return obj1.FilePath.CompareTo(obj2.FilePath) == 0 ? obj1 : obj2;
        }

        private static ConcurrentBag<SoldakObject> GenerateSoldakObjects(ConcurrentDictionary<string, string> Files, bool AllowDuplicates = false)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var newList = new ConcurrentBag<SoldakObject>();

            Debug.Log(Files.Values.Count);

            Parallel.ForEach(Files, (file) => {
                newList.Add(new SoldakObject(file.Key, file.Value));
            });

            if (!AllowDuplicates) {
                var nonDuplicateList = new HashSet<SoldakObject>();

                foreach (var obj in newList) {
                    // if (!nonDuplicateList.Contains(obj)) {
                    nonDuplicateList.Add(obj);
                    // }
                    /*
                     else {
                         nonDuplicateList.
                         nonDuplicateList.Remove(obj);

                         nonDuplicateList.Add(ReturnBasedOnFilePath(obj));
                     }
                     */
                }
                Debug.Log(nonDuplicateList.Count);

                stopwatch.Stop();
                Debug.Log("Creating objects and Removing duplicate objects took " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

                return new ConcurrentBag<SoldakObject>(nonDuplicateList);
            }
            else {
                stopwatch.Stop();
                Debug.Log("Creating objects and Not removing any duplicates took " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

                return newList;
            }
        }

        private static ConcurrentDictionary<string, string> GetAllGDBFilesInsideZip(string file)
        {
            var dict = new ConcurrentDictionary<string, string>();

            var options = new ReadOptions
            {
                Encoding = Encoding.GetEncoding("UTF-8")
            };

            if (file.EndsWith(".zip")) {
                // File.GetAccessControl(file);
                var entries = ZipFile.Read(file, options);
                foreach (var zippedFile in entries) {
                    if (zippedFile.FileName.EndsWith(".gdb")) {
                        var stream = new StreamReader(zippedFile.OpenReader());
                        var fileText = stream.ReadToEnd();
                        dict.TryAdd(fileText, file);
                        stream.Close();
                    }
                }
            }

            return dict;
        }

        public static ConcurrentDictionary<string, string> GetAllGDBFilesInFolder(string path, bool OnlyVanillaAssets = false)
        {
            var dict = new ConcurrentDictionary<string, string>();

            Parallel.ForEach(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories), (file) => {
                if (OnlyVanillaAssets) {
                    if (file.Contains("assets")) {
                        foreach (var item in GetAllGDBFilesInsideZip(file)) {
                            dict.TryAdd(item.Key, item.Value);
                        }
                    }
                }
                else {
                    foreach (var item in GetAllGDBFilesInsideZip(file)) {
                        dict.TryAdd(item.Key, item.Value);
                    }
                    if (file.EndsWith(".gdb")) {
                        dict.TryAdd(File.ReadAllText(file), file);
                    }
                }
            });

            return dict;
        }
    }
}