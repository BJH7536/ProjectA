using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 최대 중첩 횟수
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public bool AddItem(Item item)
    {
        // 모든 아이템 슬롯을 대상으로 진행
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            // i 번째 InventorySlot을 가져와서
            InventorySlot slot = inventorySlots[i];
            // 해당 Slot에 있는 InventoryItem을 가져옴
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            // 가져온 아이템이 null이 아니고, 같은 아이템이고, 갯수가 4보다 작다면
            // 아이템 갯수 증가
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
            Debug.Log(itemInSlot);
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}