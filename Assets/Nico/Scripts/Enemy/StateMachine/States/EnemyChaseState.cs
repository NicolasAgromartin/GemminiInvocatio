using System;
using System.Collections;
using UnityEngine;




public class EnemyChaseState : EnemyBaseState
{
    public override event Action<EnemyEvents> OnEventOccurred;


    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }







    public override void EnterState()
    {
        base.EnterState();
        agent.speed = 4f;
        animator.SetFloat("Movement", 1f);
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void UpdateState()
    {
        if (targetsDetector.SelectedTarget != null)
        {
            agent.SetDestination(targetsDetector.SelectedTarget.transform.position);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                OnEventOccurred?.Invoke(EnemyEvents.Attack);
            }
        }
        else
        {
            OnEventOccurred?.Invoke(EnemyEvents.TargetLost);
        }
    }



}

