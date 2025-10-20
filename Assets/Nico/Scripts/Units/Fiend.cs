using System.Collections;
using UnityEngine;
using UnityEngine.AI;







public class Fiend : Unit
{
    [SerializeField] protected FiendSO data;

    [SerializeField] protected float patrolSpeed;
    [SerializeField] protected float chaseSpeed;


    protected Animator animator;
    protected NavMeshAgent agent;
    protected AnimationEvents animationEvents;



    protected virtual void Awake()
    {
        Stats = new(data.stats);

        agent = GetComponent<NavMeshAgent>();


        InstantiateModel();
    }

    private void InstantiateModel()
    {
        if (GetComponentInChildren<Animator>() == null)
            Instantiate(data.modelPrefab, transform, false);

        animationEvents = GetComponentInChildren<AnimationEvents>();
        animator = GetComponentInChildren<Animator>();
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
}

