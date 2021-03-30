using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Slot[] equippedSlots;
    [SerializeField]
    private Slot[] inventorySlots;

    private void Start()
    {
        equippedSlots = GameManager.UI.GetElement("EquippedSlots")
            .GetComponentsInChildren<Slot>(true);
        inventorySlots = GameManager.UI.GetElement("InventorySlots")
            .GetComponentsInChildren<Slot>(true);
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].IsEmpty)
            {
                inventorySlots[i].Add(item);
                return;
            }
        }
    }
}
