using System.Collections;
using UnityEngine;
using UnityEngine.AI;




public class PlayerMinion : Fiend
{
    private Weapon weapon;
    private GameObject player;
    private ControlCrown controlCrown;
    //private readonly float maxRange = 20f;
    private readonly float timeReaction = 1f;
    private bool canMove = false;





    new private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Entity");

        if (data != null) base.Awake();

        Destroy(GetComponent<Remains>());
        Destroy(GetComponent<SphereCollider>());

        agent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<Player>().gameObject;
        
        weapon = GetComponentInChildren<Weapon>();
        weapon.GetComponent<SphereCollider>().enabled = false;
        weapon.SetIsEnemy(false);

        GetComponent<CapsuleCollider>().enabled = true;

        controlCrown = GetComponentInChildren<ControlCrown>(true);
        controlCrown.gameObject.SetActive(true);

        animationEvents = GetComponentInChildren<AnimationEvents>();
    }
    private void OnEnable()
    {
        animationEvents.OnResurrectAnimationEnd += EnableMovement;
        animationEvents.OnDeathAnimationEnd += DestroyGameObject;

        base.OnDamageRecieved += HandleDamage;
        base.OnDeath += HandleDeath;
    }
    private void OnDisable()
    {
        animationEvents.OnResurrectAnimationEnd -= EnableMovement;
        animationEvents.OnDeathAnimationEnd -= DestroyGameObject;

        base.OnDamageRecieved += HandleDamage;
        base.OnDeath -= HandleDeath;
    }
    private void Update()
    {
        animator.SetFloat("Movement", agent.velocity.magnitude);
    }







    private void HandleDamage(Unit minion, int currentHealth)
    {
        if (currentHealth > 0) animator.SetTrigger("Hurt");
    }
    private void HandleDeath(Unit minion)
    {
        animator.SetBool("IsDead", true);
        tag = "Untagged";

        Debug.Log($"{minion} is dead");
    }
    private void DestroyGameObject()
    {
        Debug.Log("Destroy GameObject");
        Destroy(gameObject);
    }



    public void SetMinionData(FiendSO data)
    {
        this.data = data;
        base.Awake();
        
        animator.ResetTrigger("Attack");
        animator.SetBool("IsDead", false);

        WeakenByResurrection();
    }
    private void WeakenByResurrection()
    {
        Stats.Attack /= 2;
        Stats.MaxHealth /= 2;
    }

    // bloquear el movimiento hasta que haya revivido
    private void EnableMovement() => canMove = true;





    public void ReturnToPlayer()
    {
        if (!canMove) return;

        StopAllCoroutines();
        StartCoroutine(FollowTarget(player));
    }
    public void AttackTarget(GameObject target)
    {
        if (!canMove) return;

        StopAllCoroutines();
        StartCoroutine(FollowTarget(target));
    }
    public void MoveToPosition(Vector3 position)
    {
        Debug.Log($"move to {position}");
        if (!canMove) return;

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

        while (target != null && Vector3.Distance(transform.position, target.transform.position) <= data.attackRange)
        {
            if (!target.CompareTag("Enemy")) yield break;
            //Debug.Log($"{gameObject.name} is attacking {target.name}");

            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(3f);
        }
    }











    //#region Wander Around
    //private IEnumerator WanderAround()
    //{
    //    while (CheckPlayerDistance())
    //    {
    //        yield return new WaitForSeconds(1f);
    //        agent.SetDestination(GenerateRandomPosition(player.transform.position));
    //    }
    //}
    //private Vector3 GenerateRandomPosition(Vector3 origin)
    //{
    //    return new Vector3(
    //        Random.Range(origin.x - maxRange/2, origin.x + maxRange/2),
    //        origin.y,
    //        Random.Range(origin.z - maxRange/2, origin.z + maxRange/2));
    //}
    //private bool CheckPlayerDistance() => Vector3.Distance(transform.position, player.transform.position) <= maxRange;
    //#endregion
}
