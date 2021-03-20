﻿using System.Collections.Generic;
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

        float distance = Vector2.Distance(transform.position, playerOffset.position);
        if (distance > attackRange)
            return;
        
        PlayAttack();
    }

    protected override void Movement()
    {
        if (Vector2.Distance(transform.position, playerOffset.position) <= viewDistance && path.Count <= 0)
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
        transform.position += (Vector3)movement * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, pos) < .1f)
            path.Pop();
    }

    public override void Attack()
    {
        StartCoroutine(player.Damage(10, direction));
    }
}
