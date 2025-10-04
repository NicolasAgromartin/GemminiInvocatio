using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;




public class Enemy : Fiend
{
    public delegate void EnemyEvent();
    public event EnemyEvent OnDamageRecievd;
    public event EnemyEvent OnFinalBossDefeated;

    private int maxHealth;
    private BehaviorGraphAgent behaviorAgent;

    public bool isFinalBoos = false;


    [Header("UI")]
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private Image healthBar;




    new private void Awake()
    {
        base.Awake();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        maxHealth = Stats.CurrentHealth;
        name = data.name;
    }
    private void Start()
    {
        SetBehaviorGraphVariables();
    }
    private void LateUpdate()
    {
        enemyCanvas.transform.LookAt(transform.position + Camera.main.transform.forward);
    }











    private void SetBehaviorGraphVariables()
    {
        behaviorAgent.GetVariable("PatrolSpeed", out BlackboardVariable<float> patrolSpeed);
        behaviorAgent.GetVariable("AttackDistance", out BlackboardVariable<float> attackDistance);
        behaviorAgent.GetVariable("TimeBetweenAttacks", out BlackboardVariable<float> timeBetweenAttacks);

        patrolSpeed.Value = agent.speed;
        attackDistance.Value = data.attackRange;
        timeBetweenAttacks.Value = data.timeBetweenAttacks;
    }

    public override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);
        OnDamageRecievd?.Invoke();

        healthBar.fillAmount = (float)Stats.CurrentHealth / maxHealth;


        if (Stats.CurrentHealth <= 0)
        {
            if (!isFinalBoos) MakeRemains();
            else
            {
                OnFinalBossDefeated?.Invoke();
            }
        }
    }





    private void MakeRemains()
    {
        enemyCanvas.gameObject.SetActive(false);
        tag = "Remains";
        name += " - Remains";
        Destroy(GetComponent<BehaviorGraphAgent>());
        Destroy(GetComponentInChildren<TargetsDetector>().gameObject);
        Remains remains = this.AddComponent<Remains>();
        remains.SetRemainsData(data);
        Destroy(this);
    }
}
