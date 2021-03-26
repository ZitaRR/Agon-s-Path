using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PauseState : State
{
    public PauseState(State previous) : base(previous)
    {

    }

    public override void Start()
    {
        base.Start();

        GameManager.SetTime(0f);
        GameManager.UI.DisableAll();
        GameManager.UI.Enable("PauseMenu");
    }

    public override void End()
    {
        base.End();

        GameManager.SetTime(1f);
        GameManager.UI.DisableAll();
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            StateMachine.SetState(Previous, Previous is SpellState);
    }
}
