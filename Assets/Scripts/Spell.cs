using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Agon's Path/Spell")]
public class Spell : ScriptableObject
{
    public Color Colour { get => colour; }

    public Entity Entity { get; private set; }
    public KeyCode Key { get => key; }

    [SerializeField]
    private GameObject spell;
    [SerializeField]
    private Color colour;
    [SerializeField]
    private new string name;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float cost;
    [SerializeField]
    private KeyCode key;

    public void Initialize(Entity entity)
    {
        Entity = entity;
    }

    public void Spawn()
    {
        if (!Entity.Mana.Decrease(cost))
            return;

        var projectile = MonoBehaviour.Instantiate(spell, Entity.transform.position, Quaternion.identity)
            .GetComponent<Projectile>();
        projectile.Initialize(
            CameraBehaviour.MouseWorldPosition,
            damage * Entity.SpellMulitplier.TotalValue,
            colour);
    }
}
