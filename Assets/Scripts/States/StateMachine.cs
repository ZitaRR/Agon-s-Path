using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateMachine 
{
    public static State State { get; private set; }

    public static void SetState(State state)
    {
        if (State == state)
            return;

        Debug.Log($"Changed state from {State?.Name ?? "None"} to {state.Name}");
        State?.End();
        State = state;
        State.Start();
    }
}
