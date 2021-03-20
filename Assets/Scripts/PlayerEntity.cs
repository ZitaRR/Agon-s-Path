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

        var mouse = CameraBehaviour.MouseWorldPosition;
        if (transform.position.x > mouse.x)
            renderer.flipX = true;
        else if (transform.position.x < mouse.x)
            renderer.flipX = false;

        direction = mouse - (Vector2)transform.position;
        print(direction);
    }

    public override void Attack()
    {
        var collider = Physics2D.OverlapCircle(CameraBehaviour.MouseWorldPosition, .5f, targetMask);

        if (!collider)
            return;

        var distance = Vector2.Distance(transform.position, collider.transform.position);
        print($"Hit {collider.name} from a distance of {distance}");

        if (distance > attackRange)
            return;

        var entity = collider.GetComponent<IDamagable>();

        if (entity is null || entity is PlayerEntity)
            return;

        StartCoroutine(entity.Damage(10, direction * 0));
    }
}
