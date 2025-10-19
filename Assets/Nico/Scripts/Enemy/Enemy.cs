using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class Enemy : Fiend
{
    private Outline outline;
    private EnemyContext context;
    private EnemyStateMachine stateMachine;
    private TargetsDetector targetsDetector;

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

        context = new(animator, transform, animationEvents, Stats, targetsDetector, agent, patrollingPoints);
        stateMachine = new(context);
    }
    private void Start()
    {
        stateMachine.Start();   
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

        currentState.transform.LookAt(Camera.main.transform);
        currentState.transform.Rotate(0, 180f, 0); // Para que no se vea al revés
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
