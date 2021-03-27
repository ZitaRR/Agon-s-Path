using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    public State Previous { get; }
    public string Name { get; }

    protected PlayerEntity player;
    protected CameraBehaviour camera;

    public State()
    {
        Name = GetType().Name.Replace("State", "");

        Previous = StateMachine.State;
        player = GameManager.Player;
        camera = GameManager.Camera;
    }

    public virtual void Start()
    {
        GameManager.OnFrame += Update;
    }

    public virtual void Continue()
    {
        GameManager.OnFrame += Update;
    }

    public virtual void End()
    {
        GameManager.OnFrame -= Update;
    }

    protected abstract void Update();
}
