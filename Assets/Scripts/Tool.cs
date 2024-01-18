using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolTypes;

public abstract class Tool : MonoBehaviour
{
    public toolTypes type;
    public float toolDamage;
    public abstract toolTypes getToolType();
    public abstract float getToolDamage();

}
