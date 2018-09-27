using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoldakModdingTool
{
    public static class Extensions
    {
        public static List<string> ToStringList(this List<SoldakObject> objects)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<string>();
            objects.ForEach(x => list.Add(x.GetTextRepresentation(x.Dict, Save.file.ModName, x.Modifier)));

            stopwatch.Stop();
            Debug.Log("Turning Objects into strings took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return list;
        }

        public static string JoinIntoString(this List<SoldakObject> objects)
        {
            return string.Join("\n", objects.ToStringList().ToArray());
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            int i = 0;

            foreach (TSource element in source) {
                if (predicate(element))
                    return i;

                i++;
            }

            return -1;
        }
    }
}