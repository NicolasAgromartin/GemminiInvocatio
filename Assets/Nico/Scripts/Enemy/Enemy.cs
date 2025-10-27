using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class Enemy : Fiend
{
    private Outline outline;
    private EnemyContext context;
    private EnemyStateMachine stateMachine;
    private TargetsDetector targetsDetector;
    private UnitAudio unitAudio;

    public TMP_Text currentState;

    [Header("Patrol")]
    [SerializeField] private List<Transform> patrollingPoints = new();




    #region Life Cycle
    new private void Awake()
    {
        base.Awake();
        name = data.name;

        outline = GetComponentInChildren<Outline>();
        targetsDetector = GetComponentInChildren<TargetsDetector>();
        unitAudio = GetComponentInChildren<UnitAudio>();

        context = new(animator, transform, animationEvents, Stats, targetsDetector, agent, patrollingPoints, patrolSpeed, chaseSpeed);
        stateMachine = new(context);
    }
    private void Start()
    {
        stateMachine.Start();
        GetComponentInChildren<Weapon>().SetWeaponDamage(Stats.Attack);
    }
    private void OnEnable()
    {
        stateMachine.OnEnable();
        stateMachine.OnStateChange += HandleStateChange;
    }
    private void OnDisable()
    {
        stateMachine.OnDisable();
        stateMachine.OnStateChange -= HandleStateChange;
    }
    private void Update()
    {
        stateMachine.Update();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        stateMachine.OnTriggerEnter(other);
    }
    #endregion





    #region Outline
    public void MarkEnemy() => outline.enabled = true;
    public void DismarkEnemy() => outline.enabled = false;
    #endregion




    private void HandleStateChange(EnemyBaseState state)
    {
        currentState.text = state.ToString();

        if(state is EnemyPatrolState)
        {
            unitAudio.PlayPatrollingSounds();
        }
        else
        {
            unitAudio.StopPatrollingSounds();
        }
        
        if(state is EnemyDeadState)
        {
            MakeRemains();
        }
    }

    

    private void MakeRemains()
    {
        tag = "Remains";
        name += " - Remains";

        Remains remains = gameObject.AddComponent<Remains>();
        remains.SetRemainsData(data);

        GetComponent<CapsuleCollider>().enabled = false;

        Destroy(GetComponentInChildren<TargetsDetector>().gameObject);
        Destroy(outline);
        Destroy(this);
    }
    

}
