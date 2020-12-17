using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerEntity : Entity
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
            Attack();
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
        {
            animator.SetBool("Walking", true);
        }
        else animator.SetBool("Walking", false);
    }

    protected override void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        animator.Play("Attack");
    }
}
