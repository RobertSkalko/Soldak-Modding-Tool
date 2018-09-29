using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoldakModdingTool
{
    public static class GetDerived
    {
        public static List<SoldakObject> GetDerivedObjectsOf(this List<SoldakObject> objects, string baseType)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool ThereIsAnotherInheritanceLevel = true;
            var derivedObjects = new List<SoldakObject>();
            List<string> Bases = new List<string>() { baseType };

            for (int i = objects.Count - 1; i > -1; i--) {
                var obj = objects[i];
                if (obj == null || !obj.HasBase) {
                    objects.RemoveAt(i);
                }
            }

            while (ThereIsAnotherInheritanceLevel) {
                ThereIsAnotherInheritanceLevel = false;

                for (int i = objects.Count - 1; i > -1; i--) {
                    var obj = objects[i];
                    if (Bases.Contains(obj.GetBase)) {
                        derivedObjects.Add(obj);
                        objects.RemoveAt(i);
                        ThereIsAnotherInheritanceLevel = true;

                        if (!Bases.Contains(obj.Name)) {
                            Bases.Add(obj.Name);
                        }
                    }
                }
            }
            stopwatch.Stop();
            Debug.Log("Getting Derived Objects Took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return derivedObjects;
        }
    }
}