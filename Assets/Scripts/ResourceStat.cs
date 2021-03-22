using System;
using UnityEngine;

[Serializable]
public sealed class ResourceStat : AttributeStat
{
    public delegate void DepletionDelegate(ResourceStat stat);
    public event DepletionDelegate OnDepletion;

    public float Value
    {
        get => value;
        set
        {
            this.value = Mathf.Clamp(value, 0, TotalValue);
            if (this.value == 0)
                OnDepletion?.Invoke(this);
        }
    }
    public bool IsEmpty { get => Value == 0; }

    [SerializeField]
    private float value;

    public ResourceStat(string name, float baseValue) 
        : base(name, baseValue) 
    {
        value = baseValue;
    }
}
