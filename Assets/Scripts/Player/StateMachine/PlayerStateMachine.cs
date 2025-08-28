using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateMachine : BaseStateMachine
{

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnemyDetector enemyDetector;
    [SerializeField] private PlayerCanvas playerCanvas;
    [SerializeField] private Player player;


    [Header("UI")]
    [SerializeField] private TMP_Text stateIndicator;

    #region States
    private PlayerIdleState idleState;
    private PlayerInteractState interactState;
    private PlayerMovementState movementState;
    private PlayerCombatState combatState;
    private PlayerHurtState hurtState;
    private PlayerDeadState deadState;
    private PlayerTacticsState tacticsState;
    #endregion

    private readonly Dictionary<(BaseState, PlayerEvent), BaseState>  eventMap = new();



    #region Life Cykle
    private void Awake()
    {
        idleState = new(this);
        movementState = new(this);
        combatState = new(this);
        deadState = new(this);
        hurtState = new(this);
        interactState = new(this);
        tacticsState = new(this);

        GenerateEventMap();
    }
    private void OnEnable()
    {
        SubscribeStateEvents();
        InputManager.OnSwarmTargetButtonPressed += SwarmEnemy;
        player.OnDamageRecieved += RecieveDamage;
    }
    private void OnDisable()
    {
        UnsubscribeStateEvents();

        InputManager.OnSwarmTargetButtonPressed -= SwarmEnemy;
        player.OnDamageRecieved -= RecieveDamage;
    }
    private void Start()
    {
        CurrentState = idleState;
        CurrentState.EnterState();
        stateIndicator.text = CurrentState.ToString();
    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        CurrentState.UpdateState();
    }
    #endregion



    #region State Events
    private void GenerateEventMap()
    {
        // IdleState es el estado por defecto, por lo que no tiene un fin
        // todas las salidas de este estado se determinan por un evento activo
        eventMap.Add((idleState, PlayerEvent.Move), movementState);
        eventMap.Add((idleState, PlayerEvent.Attack), combatState);
        eventMap.Add((idleState, PlayerEvent.Interact), interactState);
        eventMap.Add((idleState, PlayerEvent.RecieveDamage), hurtState);
        eventMap.Add((idleState, PlayerEvent.Tactics), tacticsState);

        // MoveState transiciones
        eventMap.Add((movementState, PlayerEvent.RecieveDamage), hurtState);
        eventMap.Add((movementState, PlayerEvent.End), idleState); // deja de recibir inputs de movimiento

        // AttackState transiciones
        eventMap.Add((combatState, PlayerEvent.RecieveDamage), hurtState);
        eventMap.Add((combatState, PlayerEvent.End), idleState); // finaliza la cadena de ataques

        // InteractState transiciones
        eventMap.Add((interactState, PlayerEvent.End), idleState); // finaliza el estado de interaccion
        eventMap.Add((interactState, PlayerEvent.RecieveDamage), hurtState);


        // TacticsState transitions
        eventMap.Add((tacticsState, PlayerEvent.Tactics), idleState); // exit tactics mode
        eventMap.Add((tacticsState, PlayerEvent.RecieveDamage), hurtState);

        // HurtState transiciones
        eventMap.Add((hurtState, PlayerEvent.Die), deadState); // si recibo daño y me muero
        eventMap.Add((hurtState, PlayerEvent.End), idleState); // si dejo de recibir daño pero sigo vivo

        // DeadState no tiene transiciones
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
    private void TriggerEventTransition(PlayerEvent playerEvent)
    {
        if(eventMap.TryGetValue((CurrentState, playerEvent), out BaseState nextState))
        {
            ChangeState(nextState);
        }
        stateIndicator.text = CurrentState.ToString();
    }
    #endregion





    #region Getters
    public CharacterController CharacterController => characterController;
    public CameraController CameraController => cameraController;
    public Transform CharacterModel => model;
    public Animator Animator => animator;
    public EnemyDetector EnemyDetector => enemyDetector;
    public PlayerCanvas PlayerCanvas => playerCanvas;
    #endregion


    private void RecieveDamage()
    {
        TriggerEventTransition(PlayerEvent.RecieveDamage);
    }
    private void SwarmEnemy()
    {
        //Debug.Log("swarming", TacticsSystem.SelectedEnemy);
        Debug.Log("swarming: " + (TacticsSystem.SelectedEnemy != null ? TacticsSystem.SelectedEnemy.name : "null"));

        if (CurrentState == deadState || CurrentState == interactState) return;

        TacticsSystem.SwarmEnemy(TacticsSystem.SelectedEnemy);
    }
}


