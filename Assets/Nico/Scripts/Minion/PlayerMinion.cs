using System.Collections;
using UnityEngine;
using UnityEngine.AI;




public class PlayerMinion : Fiend
{
    private AttackPerformer attackPerformer;
    private GameObject player;

    private readonly float attackWindowTime = .5f;
    private readonly float maxRange = 20f;
    private readonly float timeReaction = 1f;






    new private void Awake()
    {
        if (data != null) base.Awake();

        Destroy(GetComponent<Remains>());
        Destroy(GetComponent<SphereCollider>());

        attackPerformer = GetComponentInChildren<AttackPerformer>(true);
        attackPerformer.enabled = true;

        agent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<Player>().gameObject;

    }



    public void SetMinionData(FiendSO data)
    {
        this.data = data;
        base.Awake();
        WeakenByResurrection();
    }
    private void WeakenByResurrection()
    {
        base.GetModelMaterials();
        Stats.Attack /= 2;
        Stats.MaxHealth /= 2;
    }








    public void ReturnToPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(FollowTarget(player));
    }
    public void AttackTarget(GameObject target)
    {
        StopAllCoroutines();
        StartCoroutine(FollowTarget(target));
    }
    public void MoveToPosition(Vector3 position)
    {
        StopAllCoroutines();
        agent.SetDestination(position);
    }




    private IEnumerator FollowTarget(GameObject target)
    {
        StartCoroutine(KeepLookingAt(target));

        while (target != null)
        {
            yield return StartCoroutine(Chase(target));
            yield return null;
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

            yield return new WaitForSeconds(timeReaction);
        }
    }

    private IEnumerator Chase(GameObject target)
    {
        agent.SetDestination(target.transform.position);

        yield return new WaitForSeconds(timeReaction);

        if (target != null && target.CompareTag("Enemy"))
        {
            yield return StartCoroutine(PerformAttack(target));
        }
    }

    private IEnumerator PerformAttack(GameObject target)
    {
        Debug.Log($"{gameObject.name} is attacking {target.name}");

        while (target != null && Vector3.Distance(transform.position, target.transform.position) <= data.attackRange)
        {
            yield return StartCoroutine(attackPerformer.PerformAttack(attackWindowTime));
            yield return new WaitForSeconds(data.timeBetweenAttacks);
        }
    }






    #region Damage
    public override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        if(Stats.CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion






    #region Wander Around
    private IEnumerator WanderAround()
    {
        while (CheckPlayerDistance())
        {
            yield return new WaitForSeconds(1f);
            agent.SetDestination(GenerateRandomPosition(player.transform.position));
        }
    }
    private Vector3 GenerateRandomPosition(Vector3 origin)
    {
        return new Vector3(
            Random.Range(origin.x - maxRange/2, origin.x + maxRange/2),
            origin.y,
            Random.Range(origin.z - maxRange/2, origin.z + maxRange/2));
    }
    private bool CheckPlayerDistance() => Vector3.Distance(transform.position, player.transform.position) <= maxRange;
    #endregion
}
