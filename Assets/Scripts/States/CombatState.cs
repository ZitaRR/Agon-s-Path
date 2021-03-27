using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CombatState : State
{
    public CombatState() : base()
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
            StateMachine.SetState(new PauseState());
        else if (Input.GetKeyUp(KeyCode.I))
            StateMachine.SetState(new PauseState("Inventory"));

        camera.Combat();
        player.Movement();

        if (Input.GetKeyUp(KeyCode.Mouse0))
            player.PlayAttack();

        foreach (Spell spell in player.Spells)
        {
            if (Input.GetKeyUp(spell.Key))
                StateMachine.SetState(new SpellState(spell));
        }
    }
}
