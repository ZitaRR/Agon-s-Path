using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class MeleeEntity : Entity
{
    private Transform playerOffset;
    private Stack<Node> path = new Stack<Node>();

    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private Transform offset;

    protected override void Awake()
    {
        base.Awake();
        ASTAR.SetTilemap(map);
    }

    protected override void Start()
    {
        base.Start();
        playerOffset = player.transform.GetChild(0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = Vector2.Distance(transform.position, playerOffset.position);
        if (distance > attackRange)
            return;
        
        PlayAttack();
    }

    protected override void Movement()
    {
        if (Vector2.Distance(transform.position, playerOffset.position) <= viewDistance && path.Count <= 0)
            path = ASTAR.FindPath(Vector2Int.FloorToInt(offset.position), Vector2Int.FloorToInt(playerOffset.position));
        if (path is null || path.Count <= 0)
            return;

        var pos = path.Peek().GetVectorInt() - Vector2Int.FloorToInt(offset.localPosition);
        movement = (pos - (Vector2)transform.position).normalized;
        transform.position = (Vector2)transform.position + movement * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, pos) < .1f)
            path.Pop();
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
