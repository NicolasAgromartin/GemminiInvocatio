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
    [SerializeField] private NecromancyScreen remainsCanvas;
    [SerializeField] private Necromancy necromancy;
    [SerializeField] private AudioSource stepsClip;
    [SerializeField] private Player player;

    private Inventory inventory;

    [Header("UI")]
    [SerializeField] private TMP_Text stateIndicator;





    #region States
    protected PlayerIdleState idleState;
    protected PlayerMovementState movementState;
    protected PlayerCombatState combatState;
    protected PlayerHurtState hurtState;
    protected PlayerDeadState deadState;
    private PlayerInteractState interactState;
    private PlayerTacticsState tacticsState;
    #endregion




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
        InputManager.OnUsePotionButtonPressed += UsePotion;

        player.OnDamageRecieved += RecieveDamage;
    }
    private void OnDisable()
    {
        UnsubscribeStateEvents();

        InputManager.OnSwarmTargetButtonPressed -= SwarmEnemy;
        InputManager.OnUsePotionButtonPressed -= UsePotion;

        player.OnDamageRecieved -= RecieveDamage;
        
    }
    private void OnDestroy()
    {
        CurrentState.ExitState();
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
        eventMap.Add((idleState, TransitionEvent.Move), movementState);
        eventMap.Add((idleState, TransitionEvent.Attack), combatState);
        eventMap.Add((idleState, TransitionEvent.Interact), interactState);
        eventMap.Add((idleState, TransitionEvent.RecieveDamage), hurtState);
        eventMap.Add((idleState, TransitionEvent.Tactics), tacticsState);

        // MoveState transiciones
        eventMap.Add((movementState, TransitionEvent.RecieveDamage), hurtState);
        eventMap.Add((movementState, TransitionEvent.End), idleState); // deja de recibir inputs de movimiento

        // AttackState transiciones
        eventMap.Add((combatState, TransitionEvent.RecieveDamage), hurtState);
        eventMap.Add((combatState, TransitionEvent.End), idleState); // finaliza la cadena de ataques

        // InteractState transiciones
        eventMap.Add((interactState, TransitionEvent.End), idleState); // finaliza el estado de interaccion
        eventMap.Add((interactState, TransitionEvent.RecieveDamage), hurtState);


        // TacticsState transitions
        eventMap.Add((tacticsState, TransitionEvent.Tactics), idleState); // exit tactics mode
        eventMap.Add((tacticsState, TransitionEvent.RecieveDamage), hurtState);

        // HurtState transiciones
        eventMap.Add((hurtState, TransitionEvent.Die), deadState); // si recibo daño y me muero
        eventMap.Add((hurtState, TransitionEvent.End), idleState); // si dejo de recibir daño pero sigo vivo
            
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
    private void TriggerEventTransition(TransitionEvent playerEvent)
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
    public NecromancyScreen RemainsCanvas => remainsCanvas;
    public PlayerCanvas PlayerCanvas => playerCanvas;
    public Necromancy Necromancy => necromancy;
    public Inventory Inventory => inventory;
    public AudioSource StepsClip => stepsClip;
    #endregion

    #region Setters
    public void SetInventory(Inventory inventory) => this.inventory = inventory;
    #endregion




    private void RecieveDamage()
    {
        TriggerEventTransition(TransitionEvent.RecieveDamage);
    }
    private void SwarmEnemy()
    {
        //Debug.Log("swarming", TacticsSystem.SelectedEnemy);
        //Debug.Log("swarming: " + (TacticsSystem.SelectedEnemy != null ? TacticsSystem.SelectedEnemy.name : "null"));

        if (CurrentState == deadState || CurrentState == interactState) return;

        TacticsSystem.SwarmEnemy(TacticsSystem.SelectedEnemy);
    }
    private void UsePotion()
    {
        if(player.Stats.CurrentHealth == 100) { Debug.Log("Max health! "); return; }

        if (CurrentState == deadState || CurrentState == interactState) return;


        List<Item> potions = inventory.GetItems(ItemType.Potion);
        if (potions.Count > 0)
        {
            potions[0].Use(gameObject.GetComponent<Player>());
            inventory.RemoveItem(ItemType.Potion, potions[0]);
        }
        else
        {
            Debug.Log("No more potions to use");
        }
    }
}


