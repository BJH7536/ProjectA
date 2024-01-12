using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public class Hammer : Tool
{
    private toolTypes type = toolTypes.Hammer;
    private int toolDamage = 15;

    public override toolTypes getToolType()
    {
        return type;
    }

    public override int getToolDamage()
    {
        return toolDamage;
    }
}
