using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcData : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private bool isQuest;
    [Header("NPC Inform")]
    [SerializeField] public int npcId;
    [SerializeField] public bool isNpc;
    [SerializeField] public Sprite[] npcPortrait;
    [Header("Quest Inform")]
    [SerializeField] public int[] questId;
    [SerializeField] public int questIndex;

    public bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        isQuest = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        visualCue.SetActive(isQuest);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") 
        {
            playerInRange=true;
            isQuest = collider.transform.GetComponent<Player>().questManager.CheckQuest(questId[questIndex]);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange=false;
        }
    }
}
