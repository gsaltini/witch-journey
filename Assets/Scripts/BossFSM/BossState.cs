using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : ScriptableObject
{
    public abstract void Enter (BossStateMachine bossStateMachine);

    public abstract void Execute (BossStateMachine bossStateMachine);

    public abstract void Exit (BossStateMachine bossStateMachine);
}
