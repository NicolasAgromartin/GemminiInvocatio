using System;
using UnityEngine;




public class EnemyIdleState : EnemyBaseState
{
    public override event Action<EnemyEvents> OnEventOccurred;


    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    private readonly int waitingTime = 3;
    private float elapsedTime;


    public override void EnterState()
    {
        base.EnterState();
        elapsedTime = 0f;
        animator.SetFloat("Movement", 0);

        if (targetsDetector.SelectedTarget != null) OnEventOccurred?.Invoke(EnemyEvents.TargetFound);
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > waitingTime)
        {
            OnEventOccurred?.Invoke(EnemyEvents.StartPatrol);
        }
    }


    protected override void TargetFound(GameObject target)
    {
        OnEventOccurred?.Invoke(EnemyEvents.TargetFound);
    }

}
