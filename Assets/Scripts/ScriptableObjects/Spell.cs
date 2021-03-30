using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Agon's Path/Spell")]
public class Spell : ScriptableObject
{
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

    public void Spawn(Entity entity)
    {
        if (!entity.Mana.Decrease(cost))
            return;

        var projectile = MonoBehaviour.Instantiate(spell, entity.transform.position, Quaternion.identity)
            .GetComponent<Projectile>();
        projectile.Initialize(
            CameraBehaviour.MouseWorldPosition,
            damage * entity.SpellMulitplier.TotalValue,
            colour);
    }
}
