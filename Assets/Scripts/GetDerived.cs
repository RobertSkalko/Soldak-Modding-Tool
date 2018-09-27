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
            List<string> DerivedBaseTypes = new List<string>();
            var derivedObjects = new List<SoldakObject>();

            //List<int> ObjectsToRemoveFromList = new List<int>();

            while (ThereIsAnotherInheritanceLevel) {
                ThereIsAnotherInheritanceLevel = false;
                DerivedBaseTypes = new List<string>();

                for (int i = objects.Count - 1; i > -1; i--) {
                    var obj = objects[i];

                    if (obj == null || !obj.HasBase) {
                        objects.RemoveAt(i);
                    }
                    else if (obj.GetBase.Equals(baseType) || DerivedBaseTypes.Any(x => x.Equals(obj.GetBase))) {
                        objects.RemoveAt(i);
                        ThereIsAnotherInheritanceLevel = true;
                        DerivedBaseTypes.Add(obj.GetBase);
                        derivedObjects.Add(obj);
                    }
                }
            }
            stopwatch.Stop();
            Debug.Log("Getting Derived Objects Took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");

            return derivedObjects;
        }
    }
}