using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;



public class Fiend : Unit
{
    public event Action OnDamageRecieved;
    public event Action<GameObject> OnDeath;

    [SerializeField] protected FiendSO data;

    protected NavMeshAgent agent;
     


    protected virtual void Awake()
    {
        Stats = new(data.stats);
        
        agent = GetComponent<NavMeshAgent>();
        SetAgentData();

        InstantiateModel();
    }

    private void InstantiateModel()
    {
        bool hasModel = false;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("FiendModel"))
            {
                hasModel = true;
                break;
            }
        }
        if (!hasModel) Instantiate(data.modelPrefab, transform);
    }

    private void SetAgentData()
    {
        agent.radius = data.radius;
        agent.speed = data.speed;
        agent.stoppingDistance = data.stoppingDistance;
        agent.avoidancePriority = data.priority;
    }

    public override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        OnDamageRecieved?.Invoke();

        StartCoroutine(Damaged());

        if (Stats.Health <= 0)
        {
            Debug.Log($"{this.gameObject.name} is dead");

            OnDeath?.Invoke(this.gameObject);
        }
    }

    private IEnumerator Damaged()
    {
        //material.SetColor("_BaseColor", Color.red);

        yield return new WaitForSeconds(.5f);

        //material.SetColor("_BaseColor", baseColor);
    }
}
