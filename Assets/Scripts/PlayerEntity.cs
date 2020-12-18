using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerEntity : Entity
{
    private void Update()
    {
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

        movement = new Vector2(x, y);
        rigidbody.velocity = movement.normalized * speed;

        if (rigidbody.velocity != Vector2.zero)
            direction = rigidbody.velocity.normalized;

        if (rigidbody.velocity != Vector2.zero)
        {
            animator.SetBool("Walking", true);
        }
        else animator.SetBool("Walking", false);
    }

    protected override void Attack()
    {
        var hit = Physics2D.Raycast(transform.position, direction, attackRange, targetMask);
        if (!hit)
            return;

        var t = hit.collider.transform;
        var entity = t.GetComponent<IDamagable>();

        if (entity is null)
            return;

        entity.Damage(10, direction);
    }
}
