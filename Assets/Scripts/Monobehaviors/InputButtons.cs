using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class InputButtons : MonoBehaviour
    {
        public void SetGamePathName(string s)
        {
            Save.File.GamePath = s;
        }

        public void SetFilesToEditPathName(string s)
        {
            Save.File.FilesToEditPath = s;
        }

        public void SetModName(string s)
        {
            Save.File.ModName = s;
        }

        public void SetOutputPath(string s)
        {
            Save.File.OutputPath = s;
        }

        public void SetInputCommand(string s)
        {
            Save.File.InputCommand = s;
        }
    }
}