using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;




public class EnemyIdleState : EnemyBaseState
{
    public override event Action<EnemyEvents> OnEventOccurred;


    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    private readonly int waitingTime = 3;
    private float elapsedTime;


    public override void EnterState()
    {
        elapsedTime = 0f;

        animator.SetFloat("Movement", 0);
        targetsDetector.OnTargetsUpdated += TargetFound;
    }
    public override void ExitState()
    {
        targetsDetector.OnTargetsUpdated -= TargetFound;
    }
    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > waitingTime)
        {
            OnEventOccurred?.Invoke(EnemyEvents.StartPatrol);
        }
    }


    // transicionar a attack
    // transicionar a chase



    private void TargetFound(GameObject target)
    {
        OnEventOccurred?.Invoke(EnemyEvents.TargetFound);
    }

}
