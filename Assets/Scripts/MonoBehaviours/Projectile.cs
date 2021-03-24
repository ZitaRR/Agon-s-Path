using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public const float SPEED = 10;

    public string Name { get => name; }

    [SerializeField]
    protected new string name;
    [SerializeField]
    protected float damage = 10;
    [SerializeField]
    protected float lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector2 target)
    {
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

        StartCoroutine(entity.Damage(damage, transform.position));
        Destroy(gameObject);
    }
}
