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
        GameManager.SetTime(.2f, .5f);
        PostProcessing.SetChromaticAberration(.7f, .5f);
    }

    public static void Disable()
    {
        if (!IsActive)
            return;

        spell = null;
        action = null;
        GameManager.SetTime(1f, .2f);
        PostProcessing.SetChromaticAberration(0f, .2f);
    }

    private static void Update()
    {
        if (!IsActive)
            return;

        if (!entity.Mana.Decrease(1f * Time.unscaledDeltaTime))
            Disable();

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
