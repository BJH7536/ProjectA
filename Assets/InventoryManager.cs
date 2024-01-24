using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // �ִ� ��ø Ƚ��
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public bool AddItem(Item item)
    {
        // ��� ������ ������ ������� ����
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            // i ��° InventorySlot�� �����ͼ�
            InventorySlot slot = inventorySlots[i];
            // �ش� Slot�� �ִ� InventoryItem�� ������
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            // ������ �������� null�� �ƴϰ�, ���� �������̰�, ������ 4���� �۴ٸ�
            // ������ ���� ����
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