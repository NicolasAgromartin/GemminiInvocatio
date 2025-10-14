using System;
using System.Collections;
using NUnit.Framework.Constraints;
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
    [SerializeReference] public BlackboardVariable<float> ChaseSpeed;
    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    private NavMeshAgent navMeshAgent;
    private GameObject attackArea;
    private Enemy enemy;
    private bool isChasing;
    private readonly float attackRange = 1f;



    protected override Status OnStart()
    {
        isChasing = true;
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        enemy = Self.Value.GetComponent<Enemy>();
        attackArea = Self.Value.GetComponentInChildren<AttackPerformer>().gameObject;

        navMeshAgent.speed = ChaseSpeed.Value;

        //Animator.Value.SetFloat("Movement", 4f, .2f, Time.deltaTime);
        Animator.Value.SetFloat("Movement", ChaseSpeed.Value);

        enemy.StartCoroutine(FollowTarget(SelectedTarget.Value));

        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        if (isChasing)
        {
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
    }
    protected override void OnEnd()
    {
        base.OnEnd();
        Animator.Value.SetFloat("Movement", 0f);
    }  
    


    private IEnumerator FollowTarget(GameObject target)
    {
        while (isChasing && SelectedTarget.Value != null)
        {
            if (target.CompareTag("Player") || target.CompareTag("PlayerMinion"))
            {
                navMeshAgent.SetDestination(target.transform.position);
                
                enemy.StartCoroutine(KeepLookingAt(target));

                if (Vector3.Distance(target.transform.position, Self.Value.transform.position) <= attackRange + navMeshAgent.radius)
                {
                    isChasing = false;
                }

                // por chatGPT

                float distance = Vector3.Distance(target.transform.position, Self.Value.transform.position);
                if (distance > navMeshAgent.stoppingDistance)
                {
                    navMeshAgent.SetDestination(target.transform.position);
                }
                else
                {
                    isChasing = false;
                }



            }
            yield return null;
        }

        if (SelectedTarget.Value == null) isChasing = false;
    }

    private IEnumerator KeepLookingAt(GameObject target)
    {
        Vector3 lookDirection;

        while (target != null)
        {
            lookDirection = (target.transform.position - Self.Value.transform.position).normalized;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Self.Value.transform.rotation = Quaternion.LookRotation(lookDirection);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    //private IEnumerator FollowTarget(GameObject target)
    //{
    //    while (isChasing && SelectedTarget.Value != null)
    //    {
    //        if (target.CompareTag("Player") || target.CompareTag("PlayerMinion"))
    //        {
    //            navMeshAgent.SetDestination(
    //                Vector3.Distance(attackArea.transform.position, target.transform.position) > attackRange ?
    //                target.transform.position : Self.Value.transform.position);

    //            if (Vector3.Distance(target.transform.position, Self.Value.transform.position) <= attackRange + navMeshAgent.radius)
    //            {
    //                isChasing = false;
    //            }
    //        }
    //        yield return null;
    //    }

    //    if (SelectedTarget.Value == null) isChasing = false;
    //}
}


