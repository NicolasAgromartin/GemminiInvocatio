using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;







public class Fiend : Unit
{
    [SerializeField] protected FiendSO data;

    protected Animator animator;
    protected NavMeshAgent agent;
    protected AnimationEvents animationEvents;

    protected float patrolSpeed;
    protected float chaseSpeed;

    protected GameObject model;
    protected List<Dissolver> dissolver = new();



    protected virtual void Awake()
    {
        Stats = new(data.stats);

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = data.stoppingDistance;

        patrolSpeed = data.patrolSpeed;
        chaseSpeed = data.chaseSpeed;

        InstantiateModel();
    }

    private void InstantiateModel()
    {
        if (GetComponentInChildren<Animator>() == null)
            model = Instantiate(data.modelPrefab, transform);

        animationEvents = GetComponentInChildren<AnimationEvents>();
        animator = GetComponentInChildren<Animator>();

        dissolver.AddRange(GetComponentsInChildren<Dissolver>());
    }





     

    #region Movement
    protected IEnumerator MoveToTarget(GameObject target)
    {
        StartCoroutine(KeepLookingAt(target));

        while (target != null)
        {
            agent.SetDestination(target.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator KeepLookingAt(GameObject target)
    {
        Vector3 lookDirection;

        while (target != null)
        {
            lookDirection = (target.transform.position - transform.position).normalized;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    #endregion


    public Sprite GetFiendIcon() => data.icon;
    public string GetFiendName() => data.fiendName;



    private Material[] targetMaterials;
    private void Dissolve()
    {
        model.GetComponent<Renderer>();

    }


}

