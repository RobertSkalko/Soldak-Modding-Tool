﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoldakModdingTool
{
    public class ModNameSaving : MonoBehaviour
    {
        public string Saved => Save.Instance.ModName;

        public bool Updated = false;

        public void Update()
        {
            if (Saved != null && Saved.Length > 0 && !Updated) {
                this.gameObject.GetComponentInChildren<Text>().text = Saved;
                Updated = true;
            }
        }
    }
}