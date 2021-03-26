using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class IdleState : State
{
    public IdleState(State previous) : base(previous)
    {

    }

    public override void Start()
    {
        base.Start();

        GameManager.UI.EnableSelfAndChildren("PlayerUI");
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            StateMachine.SetState(new PauseState(this));

        camera.Roaming();
        player.Movement();

        if (Input.GetKeyUp(KeyCode.Mouse0))
            player.PlayAttack();

        foreach (Spell spell in player.Spells)
        {
            if (Input.GetKeyUp(spell.Key))
                StateMachine.SetState(new SpellState(this, spell));
        }
    }
}
