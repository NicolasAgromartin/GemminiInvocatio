using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;





public abstract class EnemyBaseState
{
    public abstract event Action<EnemyEvents> OnEventOccurred;

    protected EnemyStateMachine stateMachine;
    protected EnemyContext context;

    protected Stats stats;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Transform transform;
    protected AnimationEvents animationEvents;
    protected TargetsDetector targetsDetector;
    protected List<Transform> patrollingPoints;

    protected GameObject detected;



    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        context = stateMachine.Context;

        stats = context.Stats;
        animator = context.Animator;
        agent = context.NavMeshAgent;
        transform = context.Transform;
        animationEvents = context.AnimationEvents;
        targetsDetector = context.TargetsDetector;
        patrollingPoints = context.PatrollingPoints;
    }




    public virtual void EnterState()
    {
        targetsDetector.OnTargetsUpdated += TargetFound;
    }
    public virtual void ExitState()
    {
        targetsDetector.OnTargetsUpdated -= TargetFound;
    }
    public abstract void UpdateState();
    public virtual void OnTriggerEnter(Collider other) 
    { 
    }


    protected virtual void TargetFound(GameObject target)
    {

    }
}
