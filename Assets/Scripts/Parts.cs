using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    [Header("Parts Inform")]
    //[SerializeField] private GameObject PartsPrefab;
    [SerializeField] private float PartsHP;


    public float getPartsHP()
    {
        return PartsHP;
    }

    public void setPartsHP(float value)
    {
        PartsHP= value;
    }
}
