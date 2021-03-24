using System.Collections;
using UnityEngine;

public interface IDamagable
{
    ResourceStat Health { get; }
    bool IsDead { get; }
    IEnumerator Damage(float damage, Vector2 direction);
    IEnumerator DamageEffect();
    void Kill();
}
