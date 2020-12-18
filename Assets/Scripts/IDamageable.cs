using UnityEngine;

public interface IDamagable
{
    int Health { get; }
    void Damage(int damage, Vector2 direction);
    void Kill();
}
