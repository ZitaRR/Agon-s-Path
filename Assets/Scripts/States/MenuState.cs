using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MenuState : State
{
    public MenuState(State previous) : base(previous)
    {

    }

    public override void Start()
    {
        GameManager.UI.DisableAll();
        GameManager.UI.Enable("MainMenu");
    }

    public override void End()
    {
        GameManager.UI.DisableAll();
    }

    protected override void Update()
    {

    }
}
