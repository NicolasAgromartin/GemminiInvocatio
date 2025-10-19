using System;
using System.Collections;
using UnityEngine;




[RequireComponent(typeof(CharacterController))]
public class Player : Unit
{
    public static event Action OnPlayerRestored;
    public static event Action OnPlayerLost;

    public int Lives { get; private set; } = 3;
    

    public PlayerContext PlayerContext { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }


    private Animator animator;
    private Inventory inventory;
    private Necromancy necromancy;
    private MinionOwner minionOwner;
    private EnemyDetector enemyDetector;
    private AnimationEvents animationEvents;
    private CharacterController characterController;

    private CinemachineController cinmachineController;






    #region Life Cykle
    private void Awake()
    {
        Stats = new(100, 10, 6f, 2f);

        necromancy = GetComponent<Necromancy>();
        minionOwner = GetComponent<MinionOwner>();
        animator = GetComponentInChildren<Animator>();
        enemyDetector = GetComponentInChildren<EnemyDetector>();
        characterController = GetComponent<CharacterController>();
        animationEvents = GetComponentInChildren<AnimationEvents>();
        cinmachineController = FindAnyObjectByType<CinemachineController>();
        InitiateInventory();


        PlayerContext = new(characterController, animator, transform, inventory, necromancy, enemyDetector, minionOwner, animationEvents, Stats, cinmachineController);
        StateMachine = new(PlayerContext);
    }
    private void OnEnable()
    {
        StateMachine.OnEnable();

        RespawnManager.OnPlayerRespawned += RestorePlayer;

        animationEvents.OnDeathAnimationEnd += CheckLives;
    }
    private void OnDisable()
    {
        StateMachine.OnDisable();

        RespawnManager.OnPlayerRespawned -= RestorePlayer;

        animationEvents.OnDeathAnimationEnd -= CheckLives;
    }
    private void Start()
    {
        StateMachine.Start();
    }
    private void Update()
    {
        StateMachine.Update();
    }
    new private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        StateMachine.OnTriggerEnter(other);
    }
    #endregion






    #region Health & Life
    private void RestorePlayer()
    {
        IncreaseHealth(Stats.MaxHealth);
        tag = "Player";

        StartCoroutine(SimulateRestoreTime());
    }
    private void CheckLives()
    {
        Debug.Log("Player died");

        if (Stats.CurrentHealth <= 0)
        {
            tag = "Untagged";
            Lives--;
        }

        if (Lives <= 0)
        {
            OnPlayerLost?.Invoke();
        }
    }
    private IEnumerator SimulateRestoreTime()
    {
        yield return new WaitForSecondsRealtime(3f);
        OnPlayerRestored?.Invoke();
    }
    #endregion








    #region Inventory
    public void InitiateInventory()
    {
        inventory = new();
        necromancy.SetInventory(inventory);
    }
    public Inventory GetInventory() => inventory;
    public Item UseKey()
    {
        return inventory.GetKey();
    }
    #endregion



    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    //Gizmos.DrawWireSphere(transform.position + new Vector3(0f,1f,.8f), 1f);

    //    //Gizmos.DrawWireCube(transform.position + transform.forward * 1.5f + Vector3.up * 1f, new Vector3(1f, 2.5f, 1f));
    //}


    ////Collider[] colliders = Physics.OverlapBox(, transform.rotation, LayerMask.GetMask("Interactable"));

}
