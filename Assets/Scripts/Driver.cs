using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public class Driver : Tool
{
    private toolTypes type = toolTypes.Driver;
    private int toolDamage = 10;

    public override toolTypes getToolType() 
    {
        return type;
    }

    public override int getToolDamage()
    {
        return toolDamage;
    }
}
