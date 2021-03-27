using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Slot[] slots;

    private void Start()
    {
        Transform ui = GameManager.UI.gameObject.transform;
        slots = ui.GetComponentsInChildren<Slot>(true);
    }

    public void AddItem(Sprite item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] is null)
            {
                slots[i].Add(item);
                return;
            }
        }
    }

    public void RemoveItem(Sprite item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].name == item.name)
            {
                slots[i] = null;
                return;
            }
        }
    }

    public void RemoveItem(int index)
    {
        if (slots[index] is null)
            return;

        slots[index] = null;
    }
}
