using System;
using UnityEngine;





public class EnemyPatrolState : EnemyBaseState
{
    public override event Action<EnemyEvents> OnEventOccurred;
    


    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    private int patrolIndex;



    public override void EnterState()
    {
        animator.SetFloat("Movement", .5f);
        agent.speed = .8f;

        MoveToNextPoint();
        
        targetsDetector.OnTargetsUpdated += TargetFound;
    }
    public override void ExitState()
    {
        targetsDetector.OnTargetsUpdated -= TargetFound;
        agent.ResetPath();
    }
    public override void UpdateState()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextPoint();
        }
    }




    private void MoveToNextPoint()
    {
        if (patrollingPoints.Count == 0) return;

        agent.SetDestination(patrollingPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrollingPoints.Count;
    }
    private void TargetFound(GameObject target)
    {
        OnEventOccurred?.Invoke(EnemyEvents.TargetFound);
    }
}
