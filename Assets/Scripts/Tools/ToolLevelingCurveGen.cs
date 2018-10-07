using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolLevelingCurveGen : ToolButton
    {
        public override string Name { get => "Leveling Curve Gen"; }
        public override string Description { get; }

        private Dictionary<int, float> LevelAndCurveDict = new Dictionary<int, float>()
        {
            { 1,1.3F },
            { 10,1.1F },
            { 25,1.12F },
            { 50,1.04F },
            { 75,1.06F },
            { 90,1.09F },
        };

        private int ExpReq = 7500;
        private int maxlvl = 100;

        private SoldakObject soldakobj = new SoldakObject()
        {
            Name = "GameSystem",
            Modifier = Modifiers.overrides,
        };

        protected override void Action()
        {
            int currentlvl = 1;
            int expreq = ExpReq;

            soldakobj.Dict.Add("BaseXpNeededEachLevel", new List<string>() { ExpReq + "" });

            for (var i = 1; i <= maxlvl; i++) {
                if (LevelAndCurveDict.ContainsKey(i) && LevelAndCurveDict[i] >= currentlvl) {
                    currentlvl = i;
                }
                if (i > 1) {
                    expreq = Convert.ToInt32(expreq * LevelAndCurveDict[currentlvl]);
                }
                Debug.Log(i + " " + expreq);

                soldakobj.Dict.Add("XpMultEachLevel" + i, new List<string>() { LevelAndCurveDict[currentlvl] + " // " + expreq.ToString("N") + " Exp Required to Levelup" });
            }

            FileManager.SaveOutputToFile(soldakobj.GetTextRepresentation());
        }
    }
}