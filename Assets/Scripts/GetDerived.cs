using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoldakModdingTool
{
    public static class GetDerived
    {
        public static List<SoldakObject> GetDerivedFrom(this List<SoldakObject> objects, string baseType)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool ThereIsAnotherInheritanceLevel = true;
            var derivedObjects = new HashSet<SoldakObject>();
            HashSet<string> Bases = new HashSet<string>() { baseType };
            HashSet<string> ObjectNamesAdded = new HashSet<string>();

            for (int i = objects.Count - 1; i > -1; i--) {
                var obj = objects[i];
                if (obj == null || !obj.HasBase) {
                    objects.RemoveAt(i);
                }
            }
            while (ThereIsAnotherInheritanceLevel) {
                ThereIsAnotherInheritanceLevel = false;

                foreach (var obj in objects) {
                    if (Bases.Contains(obj.GetBase) && ObjectNamesAdded.Add(obj.Name) && Bases.Add(obj.Name) && derivedObjects.Add(obj)) {
                        ThereIsAnotherInheritanceLevel = true;
                    }
                }
            }
            stopwatch.Stop();
            Debug.Log("Getting Derived Objects Took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return derivedObjects.ToList();
        }
    }
}