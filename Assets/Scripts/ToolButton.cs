using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoldakModdingTool
{
    public abstract class ToolButton
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract void Action();
    }
}