using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public class Hammer : Tool
{
    private toolTypes Hammertype = toolTypes.Hammer;
    private float HammerDamage=15;

    public override toolTypes getToolType()
    {
        base.type = Hammertype;
        return type;
    }

    public override float getToolDamage()
    {
        base.toolDamage= HammerDamage;
        return toolDamage;
    }
}
