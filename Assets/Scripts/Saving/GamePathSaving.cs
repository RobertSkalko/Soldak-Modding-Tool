﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoldakModdingTool
{
    public class GamePathSaving : MonoBehaviour
    {
        public string Saved => Save.file.GamePath;

        public bool Updated = false;

        public void Update()
        {
            if (Saved != null && !Updated) {
                this.gameObject.GetComponentInChildren<Text>().text = Saved;
                Updated = true;
            }
        }
    }
}