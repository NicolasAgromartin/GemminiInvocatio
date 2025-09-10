using Unity.Behavior;
using Unity.VisualScripting;



public class Enemy : Fiend
{
    public delegate void EnemyEvent();
    public event EnemyEvent OnDamageRecievd;


    private BehaviorGraphAgent behaviorAgent;







    new private void Awake()
    {
        base.Awake();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
    }
    private void Start()
    {
        SetBehaviorGraphVariables();
    }












    private void SetBehaviorGraphVariables()
    {
        behaviorAgent.GetVariable("PatrolSpeed", out BlackboardVariable<float> patrolSpeed);
        behaviorAgent.GetVariable("AttackDistance", out BlackboardVariable<float> distanceThreshold);
        behaviorAgent.GetVariable("TimeBetweenAttacks", out BlackboardVariable<float> timeBetweenAttacks);

        patrolSpeed.Value = agent.speed;
        distanceThreshold.Value = data.attackRange;
        timeBetweenAttacks.Value = data.timeBetweenAttacks;
    }

    public override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);
        OnDamageRecievd?.Invoke();
        if (Stats.Health <= 0) MakeRemains();
    }





    private void MakeRemains()
    {
        tag = "Remains";
        name += " - Remains";
        Destroy(GetComponent<BehaviorGraphAgent>());
        Destroy(GetComponentInChildren<TargetsDetector>().gameObject);
        Remains remains = this.AddComponent<Remains>();
        remains.SetRemainsData(data);
        Destroy(this);
    }
}
