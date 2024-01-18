using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public class Driver : Tool
{
    private toolTypes Drivertype = toolTypes.Driver;
    public float DriverDamage = 10;

    public override toolTypes getToolType() 
    {
        base.type = Drivertype;
        return type;
    }

    public override float getToolDamage()
    {
        base.toolDamage= DriverDamage;
        return toolDamage;
    }
}
