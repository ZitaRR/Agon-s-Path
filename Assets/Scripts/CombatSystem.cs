using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatSystem
{
    public static List<Entity> Entities { get; } = new List<Entity>();
    public static bool NoEntities { get => Entities.Count <= 0; }

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

        Entities.Add(entity);
    }

    public static void RemoveCombatant(Entity entity)
    {
        if (!Exists(entity))
            return;

        Entities.Remove(entity);
    }

    public static void RemoveAllCombatants()
    {
        Entities.Clear();
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
