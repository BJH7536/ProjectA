using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemystat")]
    private float enemyHP;
    private float enemyDamage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Player의 무기를 GameManager를 통해 가져와서 전체 HP에서 빼준다.
        }
    }
}
