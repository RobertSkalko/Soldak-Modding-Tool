using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class EditorInput : MonoBehaviour
    {
        public void SetIsDerivedOf(string s)
        {
            Save.file.EditorDatas.IsDerivedOf = s;
        }

        public void SetNameContains(string s)
        {
            Save.file.EditorDatas.NameContains = s;
        }

        public void SetAnyPartContains(string s)
        {
            Save.file.EditorDatas.AnyPartContains = s;
        }

        public void SetValue(string s)
        {
            this.GetComponentInParent<EditorLine>().Value = s;
        }

        public void SetKey(string s)
        {
            this.GetComponentInParent<EditorLine>().Key = s;
        }
    }
}