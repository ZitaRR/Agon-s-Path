using System.Collections;
using UnityEngine;

public interface IDamagable
{
    float Health { get; }
    IEnumerator Damage(float damage, Vector2 direction);
    IEnumerator DamageEffect();
    void Kill();
}
