using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public sealed class PlayerEntity : Entity
{
    private new Light2D light;

    protected override void Awake()
    {
        base.Awake();
        light = GetComponentInChildren<Light2D>();
    }

    protected override void Update()
    {
        base.Update();
        light.pointLightOuterRadius = viewDistance.TotalValue;
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
        transform.position += (Vector3)movement * speed.TotalValue * Time.deltaTime;

        if (movement != Vector2.zero)
        {
            animator.SetBool("Walking", true);
        }
        else animator.SetBool("Walking", false);
    }

    protected override void Rotation()
    {
        if (!GameManager.InCombat)
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
    }

    public override void Attack()
    {
        var collider = Physics2D.OverlapCircle(CameraBehaviour.MouseWorldPosition, .5f, targetMask);

        if (!collider)
            return;

        var distance = Vector2.Distance(transform.position, collider.transform.position);
        print($"Hit {collider.name} from a distance of {distance}");

        if (distance > attackRange.TotalValue)
            return;

        var entity = collider.GetComponent<IDamagable>();

        if (entity is null || entity is PlayerEntity)
            return;

        StartCoroutine(entity.Damage(10, direction));
    }

    public override void Kill()
    {
        GameManager.Instance.LoadScene("Menu");
    }
}
