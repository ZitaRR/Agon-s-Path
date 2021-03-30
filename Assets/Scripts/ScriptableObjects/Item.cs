using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Agon's Path/Item")]
public abstract class Item : ScriptableObject
{
    public string Name { get => name; }
    public Sprite Sprite { get => sprite; }

    [SerializeField]
    private string name;
    [SerializeField]
    private Sprite sprite;

    public abstract void Use();
}
