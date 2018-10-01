using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class EditorInput : MonoBehaviour
    {
        public void SetIsDerivedOf(string s)
        {
            Save.Instance.EditorDatas.IsDerivedOf = s;
        }

        public void SetNameContains(string s)
        {
            Save.Instance.EditorDatas.NameContains = s;
        }

        public void SetNameDoesntHave(string s)
        {
            Save.Instance.EditorDatas.NameDoesntHave = s;
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