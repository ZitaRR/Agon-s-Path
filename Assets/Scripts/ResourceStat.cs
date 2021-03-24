using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class ResourceStat : AttributeStat
{
    public delegate void DepletionDelegate(ResourceStat stat);
    public event DepletionDelegate OnDepletion;

    public float Value
    {
        get => value;
        private set
        {
            this.value = Mathf.Clamp(value, 0, TotalValue);

            if (Entity is PlayerEntity)
            {
                bar.fillAmount = this.value / TotalValue;
            }

            if (this.value == 0)
                OnDepletion?.Invoke(this);
        }
    }
    public bool IsEmpty { get => Value == 0; }

    private readonly Image bar;

    [SerializeField]
    private float value;

    public ResourceStat(string name, float baseValue, Entity entity) 
        : base(name, baseValue, entity) 
    {
        if (Entity is PlayerEntity)
            bar = GameManager.UI.GetElement<Image>($"{Name}Bar");

        Value = baseValue;
        GameManager.OnFrame += Update;
        Entity.OnDestroyed += OnDestroyed;
    }

    private void Update()
    {
        if (Value >= TotalValue || Entity.IsDead)
            return;

        Value += 1f * Time.deltaTime;
    }

    public void Increase(float value)
    {
        Value += value;
    }

    public bool Decrease(float value)
    {
        if (value > Value)
            return false;

        Value -= value;
        return true;
    }

    private void OnDestroyed()
    {
        GameManager.OnFrame -= Update;
        Entity.OnDestroyed -= OnDestroyed;
    }
}
