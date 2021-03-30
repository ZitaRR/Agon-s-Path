using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Agon's Path/Consumable")]
public class Consumable : Item
{
    public override void Use()
    {
        Debug.Log($"Using consumable [{Name}]");
    }
}
