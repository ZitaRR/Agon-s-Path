using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MeleeEntity : Entity
{
    private Transform player;

    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private float meleeRange;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > meleeRange)
            return;

        Attack();
    }

    protected override void Movement()
    {
        if (rigidbody.velocity != Vector2.zero)
        {
            animator.SetBool("Walking", true);
        }
        else animator.SetBool("Walking", false);

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > viewDistance)
        {
            rigidbody.velocity = Vector2.zero;
            return;
        }

        movement = player.position - transform.position;
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            movement.y = 0;
        else movement.x = 0;

        rigidbody.velocity = movement.normalized * speed;
    }

    protected override void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        animator.Play("Attack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
