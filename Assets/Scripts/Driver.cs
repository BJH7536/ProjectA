using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public class Driver : Tool
{
    [Header("Driver Stat")]
    [SerializeField] private toolTypes Drivertype = toolTypes.Driver;
    [SerializeField] private float DriverDamage = 10;

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
