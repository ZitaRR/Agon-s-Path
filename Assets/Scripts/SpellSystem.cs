using System;
using UnityEngine;

public static class SpellSystem
{
    public static bool IsActive { get => spell != null && action != null; }

    private static PlayerEntity entity;
    private static Spell spell;
    private static Action action;

    public static void Initialize(PlayerEntity entity)
    {
        SpellSystem.entity = entity;
        GameManager.OnFrame += Update;
        SpellSystem.entity.OnDestroyed += OnDestroyed;
    }

    public static void Enable(Spell spell, Action action)
    {
        if (IsActive)
            return;

        SpellSystem.spell = spell;
        SpellSystem.action = action;
        //PostProcessing.SetChromaticAberration(.7f, 2f);
    }

    public static void Disable()
    {
        if (!IsActive)
            return;

        spell = null;
        action = null;
        //PostProcessing.SetChromaticAberration(0f, 1f);
    }

    private static void Update()
    {
        if (!IsActive)
            return;

        entity.Mana.Decrease(1f * Time.unscaledDeltaTime);
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            action();
        }
    }

    private static void OnDestroyed()
    {
        GameManager.OnFrame -= Update;
        entity.OnDestroyed -= OnDestroyed;
    }
}
