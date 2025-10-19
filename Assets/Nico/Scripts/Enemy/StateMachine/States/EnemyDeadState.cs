using System;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override event Action<EnemyEvents> OnEventOccurred;



    public override void EnterState()
    {
        animator.SetBool("IsDead", true);
    }
    public override void ExitState()
    {
    }
    public override void UpdateState()
    {
    }
}
