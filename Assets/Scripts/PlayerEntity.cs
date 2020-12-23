using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerEntity : Entity
{
    private new Light light;

    protected override void Awake()
    {
        base.Awake();
        light = GetComponentInChildren<Light>();
    }

    protected override void Update()
    {
        base.Update();
        light.spotAngle = viewDistance;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            PlayAttack();
    }

    protected override void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(x) > Mathf.Abs(y))
            y = 0;
        else x = 0;

        movement = new Vector2(x, y).normalized;
        transform.position += (Vector3)movement * speed * Time.deltaTime;

        if (movement != Vector2.zero && !CombatSystem.InCombat)
            direction = movement;

        if (movement != Vector2.zero)
        {
            animator.SetBool("Walking", true);
        }
        else animator.SetBool("Walking", false);
    }

    protected override void Rotation()
    {
        if (!CombatSystem.InCombat)
        {
            base.Rotation();
            return;
        }

        var mouse = CameraBehaviour.Camera.ScreenToWorldPoint(Input.mousePosition);
        if (transform.position.x > mouse.x)
            renderer.flipX = true;
        else if (transform.position.x < mouse.x)
            renderer.flipX = false;

        direction = mouse - transform.position;
    }

    protected override void Attack()
    {
        var hit = Physics2D.Raycast(transform.position, direction, attackRange, targetMask);
        if (!hit)
            return;

        var t = hit.collider.transform;
        var entity = t.GetComponent<IDamagable>();

        if (entity is null || entity is PlayerEntity)
            return;

        StartCoroutine(entity.Damage(10, direction));
    }
}
