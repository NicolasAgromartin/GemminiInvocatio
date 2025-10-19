using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public event Action<EnemyBaseState> OnStateChange;
    public EnemyContext Context { get; private set; }



    #region States
    public EnemyBaseState CurrentState { get; private set; }

    private EnemyIdleState idleState;
    private EnemyPatrolState patrolState;
    private EnemyChaseState chaseState;
    private EnemyAttackingState attackState;
    private EnemyHurtState hurtState;
    private EnemyDeadState deadState;

    private readonly Dictionary<(EnemyBaseState, EnemyEvents), EnemyBaseState> eventMap = new();
    #endregion


    public EnemyStateMachine(EnemyContext context)
    {
        Context = context;

        idleState = new(this);
        patrolState = new(this);
        chaseState = new(this);
        attackState = new(this);
        hurtState = new(this);
        deadState = new(this);

        GenerateEventMap();
    }




    #region Life Cycle
    public void OnEnable()
    {
        SubscribeStateEvents();
    }
    public void OnDisable()
    {
        UnsubscribeStateEvents();
        CurrentState?.ExitState();
    }
    public void Start()
    {
        CurrentState = idleState;
        CurrentState.EnterState();
    }
    public void Update()
    {
        CurrentState.UpdateState();
        if (Context.Stats.CurrentHealth <= 0)
        {
            ChangeState(deadState);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    #endregion








    #region State Transitions
    private void GenerateEventMap()
    {
        // idle
        eventMap.Add((idleState, EnemyEvents.StartPatrol), patrolState);
        eventMap.Add((idleState, EnemyEvents.TargetFound), chaseState);
        eventMap.Add((idleState, EnemyEvents.Attack), attackState);

        // patrol
        eventMap.Add((patrolState, EnemyEvents.TargetFound), chaseState);
        eventMap.Add((patrolState, EnemyEvents.End), idleState);

        // chase
        eventMap.Add((chaseState, EnemyEvents.TakeHit), hurtState);
        eventMap.Add((chaseState, EnemyEvents.Attack), attackState);
        eventMap.Add((chaseState, EnemyEvents.TargetDied), idleState);
        eventMap.Add((chaseState, EnemyEvents.TargetLost), idleState);

        // attack
        eventMap.Add((attackState, EnemyEvents.TargetLost), idleState);
        eventMap.Add((attackState, EnemyEvents.TargetMoved), chaseState);
        eventMap.Add((attackState, EnemyEvents.TakeHit), hurtState);
        eventMap.Add((attackState, EnemyEvents.Die), deadState);

        // hurt
        eventMap.Add((hurtState, EnemyEvents.Die), deadState);
        eventMap.Add((hurtState, EnemyEvents.TakeHit), idleState);
        eventMap.Add((hurtState, EnemyEvents.End), idleState);

        // dead 
    }
    private void SubscribeStateEvents()
    {
        idleState.OnEventOccurred += TriggerEventTransition;
        hurtState.OnEventOccurred += TriggerEventTransition;
        deadState.OnEventOccurred += TriggerEventTransition;
        chaseState.OnEventOccurred += TriggerEventTransition;
        patrolState.OnEventOccurred += TriggerEventTransition;
        attackState.OnEventOccurred += TriggerEventTransition;
    }
    private void UnsubscribeStateEvents()
    {
        idleState.OnEventOccurred -= TriggerEventTransition;
        hurtState.OnEventOccurred -= TriggerEventTransition;
        deadState.OnEventOccurred -= TriggerEventTransition;
        chaseState.OnEventOccurred -= TriggerEventTransition;
        patrolState.OnEventOccurred -= TriggerEventTransition;
        attackState.OnEventOccurred -= TriggerEventTransition;
    }
    private void TriggerEventTransition(EnemyEvents enemyEvent)
    {
        if (eventMap.TryGetValue((CurrentState, enemyEvent), out EnemyBaseState nextState))
        {
            ChangeState(nextState);
        }
    }
    private void ChangeState(EnemyBaseState nextState)
    {
        if (CurrentState == nextState) return;

        CurrentState.ExitState();
        CurrentState = nextState;
        CurrentState.EnterState();

        OnStateChange?.Invoke(CurrentState);
    }
    #endregion
}
