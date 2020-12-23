using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatSystem
{
    public static bool InCombat => Entities != null && Entities.Count > 0;
    public static List<Entity> Entities { get; } = new List<Entity>();

    public static bool Exists(Entity entity)
    {
        foreach (var ent in Entities)
        {
            if (ent.ID == entity.ID)
                return true;
        }
        return false;
    }

    public static void AddCombatant(Entity entity)
    {
        if (Exists(entity))
            return;
        if (!InCombat)
            GameManager.SetState(GameManager.GameState.Combat);

        Entities.Add(entity);
    }

    public static void RemoveCombatant(Entity entity)
    {
        if (!Exists(entity))
            return;

        Entities.Remove(entity);
        if (InCombat)
            GameManager.SetState(GameManager.GameState.Idle);
    }

    public static List<Vector2> GetPositions()
    {
        var entities = new List<Vector2>();
        foreach (var ent in Entities)
        {
            entities.Add(ent.transform.position);
        }
        return entities;
    }
}
