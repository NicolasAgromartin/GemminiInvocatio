using UnityEngine;
using System.Collections;




public class EnemyIA : Fiend
{

    private TargetsDetector detector;



    new private void Awake()
    {
        base.Awake();
        detector = GetComponentInChildren<TargetsDetector>();

    }

    private void OnEnable()
    {
        detector.OnTargetsUpdated += UpdateTarget;
    }
    private void OnDisable()
    {
        detector.OnTargetsUpdated -= UpdateTarget;
    }













    private void UpdateTarget(GameObject target)
    {
        if(target != null)
        {
            StartCoroutine(FollowTarget(target));
        }
        else
        {
            StopAllCoroutines();
        }
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

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Chase(GameObject target)
    {
        agent.SetDestination(target.transform.position);

        yield return new WaitForSeconds(1f);

        if (target != null && (target.CompareTag("Player") || target.CompareTag("PlayerMinion")))
        {
            yield return StartCoroutine(PerformAttack(target));
        }
    }

    private IEnumerator PerformAttack(GameObject target)
    {
        Debug.Log($"{gameObject.name} is attacking {target.name}");

        while (target != null && Vector3.Distance(transform.position, target.transform.position) <= data.attackRange)
        {
            //yield return StartCoroutine(attackPerformer.PerformAttack(attackWindowTime));
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(data.timeBetweenAttacks);
        }
    }
}
