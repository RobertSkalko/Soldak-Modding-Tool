using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class InputButtons : MonoBehaviour
    {
        public void SetGamePathName(string s)
        {
            Save.file.GamePath = s;
        }

        public void SetFilesToEditPathName(string s)
        {
            Save.file.FilesToEditPath = s;
        }

        public void SetModName(string s)
        {
            Save.file.ModName = s;
        }

        public void SetOutputPath(string s)
        {
            Save.file.OutputPath = s;
        }
    }
}