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
            //Player�� ���⸦ GameManager�� ���� �����ͼ� ��ü HP���� ���ش�.
        }
    }
}
