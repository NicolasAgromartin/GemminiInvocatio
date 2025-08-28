using System;
using System.Collections;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;




[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chasing", story: "Move [Self] to [SelectedTarget]", category: "Action", id: "fd247ed0496aee2f5393d652d3d93e0d")]
public partial class ChasingAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;

    private NavMeshAgent navMeshAgent;
    private bool isChasing;
    private readonly float attackRange = 1f;
    private Enemy enemy;



    protected override Status OnStart()
    {
        isChasing = true;
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        enemy = Self.Value.GetComponent<Enemy>();

        enemy.StartCoroutine(FollowTarget(SelectedTarget.Value));

        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        return isChasing? Status.Running : Status.Success;
    }


    private IEnumerator FollowTarget(GameObject target)
    {
        while (isChasing && SelectedTarget.Value != null)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerMinion"))
            {
                navMeshAgent.SetDestination(
                    Vector3.Distance(Self.Value.transform.position, target.transform.position) > attackRange ?
                    target.transform.position : Self.Value.transform.position);

                if (Vector3.Distance(target.transform.position, Self.Value.transform.position) <= attackRange + navMeshAgent.radius)
                {
                    isChasing = false;
                }
            }
            yield return null;
        }

        if (SelectedTarget.Value == null) isChasing = false;
    }
}

