using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public abstract void Enter (StateMachine stateMachine);

    public abstract void Execute (StateMachine stateMachine);

    public abstract void Exit (StateMachine stateMachine);
}



