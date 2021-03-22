using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttributeStat 
{
    public string Name { get; }
    public float BaseValue { get => baseValue; }
    public float TotalValue { get => totalValue; }
    public Entity Entity { get; }

    private readonly List<StatModifier> modifiers;

    [SerializeField]
    private float baseValue;
    [SerializeField]
    private float totalValue;

    public AttributeStat(string name, float baseValue, Entity entity)
    {
        Name = name;
        this.baseValue = baseValue;
        Entity = entity;

        modifiers = new List<StatModifier>();
        CalculateTotalValue();
    }

    public void AddModifier(StatModifier mod)
    {
        modifiers.Add(mod);
        CalculateTotalValue();
    }

    public void RemoveModifier(StatModifier mod)
    {
        modifiers.Remove(mod);
        CalculateTotalValue();
    }

    protected void CalculateTotalValue()
    {
        totalValue = baseValue;
        for (int i = 0; i < modifiers.Count; i++)
        {
            totalValue += modifiers[i].Value;
        }
    }
}
