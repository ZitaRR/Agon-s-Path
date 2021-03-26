using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public sealed class MeleeEntity : Entity
{
    private Transform playerOffset;
    private Stack<Node> path = new Stack<Node>();

    protected override void Awake()
    {
        base.Awake();
        Tilemap map = GameObject.Find("Walkable").GetComponent<Tilemap>();
        ASTAR.SetTilemap(map);
    }

    protected override void Start()
    {
        base.Start();
        playerOffset = player.Offset;
    }

    protected override void Update()
    {
        base.Update();

        if (IsDead)
            return;

        float distance = Vector2.Distance(transform.position, playerOffset.position);
        if (distance > attackRange.TotalValue)
            return;
        
        PlayAttack();
    }

    public override void Movement()
    {
        Rotation();

        if (Vector2.Distance(transform.position, playerOffset.position) <= viewDistance.TotalValue && path.Count <= 0)
        {
            path = ASTAR.FindPath(Vector2Int.FloorToInt(Offset.position), Vector2Int.FloorToInt(playerOffset.position));
            CombatSystem.AddCombatant(this);
        }
        if (path is null || path.Count <= 0)
        {
            CombatSystem.RemoveCombatant(this);
            return;
        }

        var pos = path.Peek().GetVectorInt() - Vector2Int.FloorToInt(Offset.localPosition);
        movement = (pos - (Vector2)transform.position).normalized;
        transform.position += (Vector3)movement * speed.TotalValue * Time.deltaTime;

        if (Vector2.Distance(transform.position, pos) < .1f)
            path.Pop();
    }

    public override void Attack()
    {
        StartCoroutine(player.Damage(10, direction));
    }
}
