using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public abstract class Tool : MonoBehaviour
{
    private toolTypes type;
    private int toolDamage;
    public abstract toolTypes getToolType();
    public abstract int getToolDamage();

}
