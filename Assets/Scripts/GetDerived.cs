using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoldakModdingTool
{
    public static class GetDerived
    {
        public static List<SoldakObject> GetDerivedObjectsOf(this List<SoldakObject> objects, string baseType)
        {
            bool ThereIsAnotherInheritanceLevel = true;
            List<string> DerivedBaseTypes = new List<string>();
            var derivedObjects = new List<SoldakObject>();

            while (ThereIsAnotherInheritanceLevel) {
                ThereIsAnotherInheritanceLevel = false;
                DerivedBaseTypes = new List<string>();

                for (int i = objects.Count - 1; i > -1; i--) {
                    var obj = objects[i];

                    if (!obj.HasBase) {
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
            return derivedObjects;
        }
    }
}