using System.Collections;
using UnityEngine;

public interface IDamagable
{
    int Health { get; }
    IEnumerator Damage(int damage, Vector2 direction);
    IEnumerator DamageEffect();
    void Kill();
}
