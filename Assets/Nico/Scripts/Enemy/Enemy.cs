using NUnit.Framework;
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
    private Outline outline;
    

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

        outline = GetComponent<Outline>();
        //outline = GetComponentInChildren<Outline>();
    }
    private void Start()
    {
        //SetBehaviorGraphVariables();
    }
    private void LateUpdate()
    {
        //enemyCanvas.transform.LookAt(transform.position + Camera.main.transform.forward);
    }


    public void MarkEnemy()
    {
        outline.enabled = true;
    }
    public void DismarkEnemy()
    {
        outline.enabled = false;
    }









    private void SetBehaviorGraphVariables()
    {
        behaviorAgent.GetVariable("ChaseSpeed", out BlackboardVariable<float> chaseSpeed);
        behaviorAgent.GetVariable("PatrolSpeed", out BlackboardVariable<float> patrolSpeed);
        behaviorAgent.GetVariable("AttackDistance", out BlackboardVariable<float> attackDistance);
        behaviorAgent.GetVariable("TimeBetweenAttacks", out BlackboardVariable<float> timeBetweenAttacks);

        patrolSpeed.Value = agent.speed;
        attackDistance.Value = data.attackRange;
        timeBetweenAttacks.Value = data.timeBetweenAttacks;
        chaseSpeed.Value = data.chaseSpeed;

        //Debug.Log($"Animator is----> {this.animator == null}");

        //animator.Value = this.animator;
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
        tag = "Remains";
        name += " - Remains";
        enemyCanvas.gameObject.SetActive(false);
        Destroy(GetComponent<BehaviorGraphAgent>());
        Destroy(GetComponentInChildren<TargetsDetector>().gameObject);

        Remains remains = this.AddComponent<Remains>();
        remains.SetRemainsData(data);
        
        Destroy(this);
    }
}
