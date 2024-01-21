using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemystat")]
    [SerializeField]private float enemyHP;
    [SerializeField]private float enemyDamage;

    
    public float getEnemyHP()
    {
       return enemyHP;
    }

    public void setEnemyHP(float value)
    {
        enemyHP = value;
    }

    public float getEnemyDamage()
    {
        return enemyDamage;
    }

    public void setEnemyDam(float value)
    {
        enemyDamage = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //{
        //    //Player의 무기를 GameManager를 통해 가져와서 전체 HP에서 빼준다. 
        //    enemyHP -= collision.GetComponent<Hammer>().HammerDamage;
        //    Debug.Log("enemyHP:"+enemyHP);
        //}
    }
}
