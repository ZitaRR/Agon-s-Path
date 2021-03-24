using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Agon's Path/Spell")]
public class Spell : ScriptableObject
{
    private Entity entity;

    [SerializeField]
    private GameObject spell;
    [SerializeField]
    private float cost;
    [SerializeField]
    private KeyCode key;

    public void Initialize(Entity entity)
    {
        this.entity = entity;

        if (this.entity is PlayerEntity)
        {
            GameManager.OnFrame += Update;
            this.entity.OnDestroyed += OnDestroyed;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(key))
        {
            if (SpellSystem.IsActive)
                SpellSystem.Disable();
            else SpellSystem.Enable(this, Spawn);
        }
    }

    private void Spawn()
    {
        if (!entity.Mana.Decrease(cost))
            return;

        var projectile = MonoBehaviour.Instantiate(spell, entity.transform.position, Quaternion.identity)
            .GetComponent<Projectile>();
        projectile.Initialize(CameraBehaviour.MouseWorldPosition);

        Debug.Log($"{entity.name} casting {projectile.Name}");
    }

    private void OnDestroyed()
    {
        GameManager.OnFrame -= Update;
        this.entity.OnDestroyed -= OnDestroyed;
    }
}
