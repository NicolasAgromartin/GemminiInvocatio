using System;
using System.Collections;
using UnityEngine;




[RequireComponent(typeof(CharacterController))]
public class Player : Unit
{
    public static event Action OnPlayerRestored;
    public static event Action OnPlayerLost;
    override public event Action<Unit> OnDeath;
    override public event Action<Unit, int> OnDamageRecieved;

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

        GetComponentInChildren<Weapon>().SetWeaponDamage(Stats.Attack);

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
    }
    private void OnDisable()
    {
        StateMachine.OnDisable();

        RespawnManager.OnPlayerRespawned -= RestorePlayer;
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
        StartCoroutine(SimulateRestoreTime());
    }

    override protected void RecieveDamage(int damage)
    {
        //Debug.Log($"{damage} recieved from player script");

        Stats.CurrentHealth -= damage;

        if (Stats.CurrentHealth <= 0) Stats.CurrentHealth = 0;

        OnDamageRecieved?.Invoke(this, Stats.CurrentHealth);




        if (Stats.CurrentHealth <= 0)
        {
            tag = "Untagged";
            Lives--;

            if (Lives <= 0)
            {
                OnDeath?.Invoke(this);
                OnPlayerLost?.Invoke();
                return;
            }

            OnDeath?.Invoke(this);
            return;
        }
    }
    private IEnumerator SimulateRestoreTime()
    {
        yield return new WaitForSecondsRealtime(3f);
        tag = "Player";
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
    public bool FindKey(int gateId)
    {
        if (Inventory.Items[ItemType.KeyItem].Count == 0) return false;

        foreach (Item key in Inventory.Items[ItemType.KeyItem])
        {
            if (key.Data is KeyItem_SO keyItem)
            {
                if (keyItem.id == gateId) return true;
                else continue;
            }
        }

        return false;
    }
    #endregion



<<<<<<< Updated upstream


    private void OnDrawGizmos()
    {
        // Parámetros del OverlapBox
        Vector3 center = transform.position + transform.forward * 1.5f + Vector3.up * 1f;
        Vector3 halfExtents = new Vector3(1f, 2.5f, 1f);

        // Dibujo del gizmo
        Gizmos.color = Color.cyan;

        // Aplicamos la rotación del objeto al dibujo
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        // Dibuja un cubo con el mismo tamaño que el OverlapBox
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);

        // (Opcional) Restaurar la matriz
        Gizmos.matrix = Matrix4x4.identity;
    }
=======
>>>>>>> Stashed changes
}
