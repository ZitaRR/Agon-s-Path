using UnityEngine;

public sealed class SpellState : State
{
    private Spell spell;

    public SpellState(State previous, Spell spell) : base(previous)
    {
        this.spell = spell;
    }

    public override void Start()
    {
        base.Start();

        GameManager.UI.EnableSelfAndChildren("PlayerUI");
        GameManager.SetTime(.2f, .5f);
        PostProcessing.SetChromaticAberration(.7f, .5f);
    }

    public override void Continue()
    {
        base.Continue();

        GameManager.UI.EnableSelfAndChildren("PlayerUI");
        GameManager.SetTime(.2f);
        PostProcessing.SetChromaticAberration(.7f);
    }

    public override void End()
    {
        base.End();

        GameManager.SetTime(1f, .2f);
        PostProcessing.SetChromaticAberration(0f, .2f);
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            StateMachine.SetState(new PauseState(this));
        else if (Input.GetKeyUp(KeyCode.I))
            StateMachine.SetState(new PauseState(this, "Inventory"));

        camera.Mouse();
        player.Movement();

        if (!player.Mana.Decrease(1f * Time.unscaledDeltaTime))
            End();

        if (Input.GetKeyUp(KeyCode.Mouse0))
            spell.Spawn();
        else if (Input.GetKeyUp(spell.Key))
            StateMachine.SetState(new IdleState(this));
    }
}
