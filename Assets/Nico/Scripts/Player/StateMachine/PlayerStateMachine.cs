using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;




public class PlayerStateMachine
{
    public static event Action<BaseState> OnStateChange;

    public PlayerContext PlayerContext { get; private set; }






    #region States
    public static BaseState CurrentState { get; private set; }

    private PlayerIdleState idleState;
    private PlayerHurtState hurtState;
    private PlayerDeadState deadState;
    private PlayerCombatState combatState;
    private PlayerTacticsState tacticsState;
    private PlayerInteractState interactState;
    private PlayerMovementState movementState;

    private readonly Dictionary<(BaseState, TransitionEvent), BaseState> eventMap = new();
    #endregion




    #region Life Cykle
    public PlayerStateMachine(PlayerContext playerContext)
    {
        PlayerContext = playerContext;

        idleState = new(this);
        movementState = new(this);
        combatState = new(this);
        deadState = new(this);
        hurtState = new(this);
        interactState = new(this);
        tacticsState = new(this);

        GenerateEventMap();
    }
    public void OnEnable()
    {
        SubscribeStateEvents();
    }
    public void OnDisable()
    {
        UnsubscribeStateEvents();
        CurrentState.ExitState();
    }
    public void Start()
    {
        CurrentState = idleState;
        CurrentState.EnterState();
    }
    public void Update()
    {
        CurrentState.UpdateState();
        if (PlayerContext.Stats.CurrentHealth <= 0)
        {
            PlayerContext.Animator.SetBool("Death", true);
            ChangeState(deadState);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    #endregion







    #region State Events
    private void GenerateEventMap()
    {
        // IdleState
        eventMap.Add((idleState, TransitionEvent.Move), movementState);
        eventMap.Add((idleState, TransitionEvent.Attack), combatState);
        eventMap.Add((idleState, TransitionEvent.Interact), interactState);
        eventMap.Add((idleState, TransitionEvent.RecieveDamage), hurtState);
        eventMap.Add((idleState, TransitionEvent.Tactics), tacticsState);
        eventMap.Add((idleState, TransitionEvent.Die), deadState);

        // MoveState
        eventMap.Add((movementState, TransitionEvent.RecieveDamage), hurtState);
        eventMap.Add((movementState, TransitionEvent.Attack), combatState);
        eventMap.Add((movementState, TransitionEvent.End), idleState); // deja de recibir inputs de movimiento

        // AttackState
        eventMap.Add((combatState, TransitionEvent.RecieveDamage), hurtState);
        eventMap.Add((combatState, TransitionEvent.End), idleState); // finaliza la cadena de ataques
        eventMap.Add((combatState, TransitionEvent.Die), deadState);

        // InteractState
        eventMap.Add((interactState, TransitionEvent.End), idleState); // finaliza el estado de interaccion
        eventMap.Add((interactState, TransitionEvent.RecieveDamage), hurtState);


        // TacticsState
        eventMap.Add((tacticsState, TransitionEvent.Tactics), idleState); // exit tactics mode
        eventMap.Add((tacticsState, TransitionEvent.RecieveDamage), hurtState);

        // HurtState
        eventMap.Add((hurtState, TransitionEvent.Die), deadState); // si recibo daño y me muero
        eventMap.Add((hurtState, TransitionEvent.End), idleState); // si dejo de recibir daño pero sigo vivo

        // DeadState
        eventMap.Add((deadState, TransitionEvent.Respawn), idleState);
    }
    private void SubscribeStateEvents()
    {
        idleState.OnEventOccurred += TriggerEventTransition;
        movementState.OnEventOccurred += TriggerEventTransition;
        combatState.OnEventOccurred += TriggerEventTransition;
        deadState.OnEventOccurred += TriggerEventTransition;
        hurtState.OnEventOccurred += TriggerEventTransition;
        interactState.OnEventOccurred += TriggerEventTransition;
        tacticsState.OnEventOccurred += TriggerEventTransition;
    }
    private void UnsubscribeStateEvents()
    {
        idleState.OnEventOccurred -= TriggerEventTransition;
        movementState.OnEventOccurred -= TriggerEventTransition;
        combatState.OnEventOccurred -= TriggerEventTransition;
        deadState.OnEventOccurred -= TriggerEventTransition;
        hurtState.OnEventOccurred -= TriggerEventTransition;
        interactState.OnEventOccurred -= TriggerEventTransition;
        tacticsState.OnEventOccurred -= TriggerEventTransition;
    }
    private void TriggerEventTransition(TransitionEvent playerEvent)
    {
        if(eventMap.TryGetValue((CurrentState, playerEvent), out BaseState nextState))
        {
            ChangeState(nextState);
        }
    }
    protected internal void ChangeState(BaseState nextState)
    {
        if (CurrentState == nextState) return;

        CurrentState.ExitState();
        CurrentState = nextState;
        CurrentState.EnterState();

        OnStateChange?.Invoke(CurrentState);
    }
    #endregion









}


