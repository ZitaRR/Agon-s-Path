using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Agon's Path/Item")]
public abstract class Item : ScriptableObject
{
    public enum ItemType
    {
        None,
        Consumable, 
        Spell,
        Weapon,
        Armor
    }

    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite Sprite { get => sprite; }
    public ItemType Type { get => type; }

    [SerializeField]
    protected new string name;
    [TextArea(0, 7)]
    [SerializeField]
    protected string description;
    [SerializeField]
    protected Sprite sprite;
    [SerializeField]
    protected ItemType type = ItemType.None;

    public abstract void Use();
}
