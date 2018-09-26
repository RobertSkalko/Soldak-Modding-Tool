using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public class ToolValidityChecker : ToolButton
    {
        public override string Name { get => "Validity Checker"; }
        public override string Description { get => "Put your mod zip file in the specified folder and it will check its validity"; }

        public override void Action()
        {
        }
    }
}