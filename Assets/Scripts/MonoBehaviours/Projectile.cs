using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    public const float SPEED = 10;

    private float damage;

    [SerializeField]
    protected float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector2 target, float damage, Color colour)
    {
        this.damage = damage;

        var light = GetComponentInChildren<Light2D>();
        if (light)
            light.color = colour;

        target.x -= transform.position.x;
        target.y -= transform.position.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update()
    {
        transform.position += transform.right * SPEED * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var entity = collider.GetComponent<IDamagable>();
        if (entity is null || entity is PlayerEntity)
            return;

        Debug.Log($"{name} dealt {damage} to {entity}");
        StartCoroutine(entity.Damage(damage, transform.position));
        Destroy(gameObject);
    }
}
