using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class EditorInput : MonoBehaviour
    {
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