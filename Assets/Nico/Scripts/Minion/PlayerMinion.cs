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

    private bool suscribedToTacicts;



    #region Life Cykle
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
    private void OnEnable()
    {
        TacticsSystem.OnQuickReturn += ReturnToPlayer;
        TacticsSystem.OnSwarmOrder += AttackTarget;

        TacticsSystem.OnPlayerMinionSelected += SuscribeToTactics; // se suscriben al tactics sistem para suscribirse a sus eventos cuando se los selecciona
        TacticsSystem.OnMinionUnselected += UnsuscribeToTactics;
    }
    private void OnDisable()
    {
        UnsuscribeToTactics();

        TacticsSystem.OnQuickReturn -= ReturnToPlayer;
        TacticsSystem.OnSwarmOrder -= AttackTarget;

        TacticsSystem.OnPlayerMinionSelected -= SuscribeToTactics;
        TacticsSystem.OnMinionUnselected -= UnsuscribeToTactics;
    }
    #endregion



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




    #region Tactics System
    private void SuscribeToTactics(GameObject minionSelected)
    {
        // el input manager detecta todos los colliders el prefab y te lleva al root de todos
        // que es donde esta el tag PlayerMinion y este script

        if (minionSelected != this.gameObject)
        {
            UnsuscribeToTactics();
            return;
        }
        suscribedToTacicts = true;
        TacticsSystem.OnReturnOrder += ReturnToPlayer;
        TacticsSystem.OnMoveToPositionOrder += MoveToPosition;
        TacticsSystem.OnTargetChangeOrder += AttackTarget;
    }
    private void UnsuscribeToTactics()
    {
        suscribedToTacicts = false;
        TacticsSystem.OnReturnOrder -= ReturnToPlayer;
        TacticsSystem.OnMoveToPositionOrder -= MoveToPosition;
        TacticsSystem.OnTargetChangeOrder -= AttackTarget;
    }
    #endregion







    #region Tactic Actions
    private void ReturnToPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(FollowTarget(player));

        if (suscribedToTacicts) UnsuscribeToTactics();
    }
    private void AttackTarget(GameObject target)
    {
        StopAllCoroutines();
        StartCoroutine(FollowTarget(target));

        if (suscribedToTacicts) UnsuscribeToTactics();
    }
    private void MoveToPosition(Vector3 position)
    {
        StopAllCoroutines();
        agent.SetDestination(position);

        if (suscribedToTacicts) UnsuscribeToTactics();
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
    //private IEnumerator PerformAttack(GameObject target)
    //{
    //    Debug.Log($"{gameObject.name} is attacking {target.name}");

    //    while (target != null && Vector3.Distance(transform.position, target.transform.position) <= data.attackRange)
    //    {
    //        // Rotar hacia el objetivo
    //        Vector3 direction = (target.transform.position - transform.position).normalized;
    //        direction.y = 0; // evitar inclinarse hacia arriba/abajo
    //        if (direction != Vector3.zero)
    //        {
    //            Quaternion lookRotation = Quaternion.LookRotation(direction);
    //            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    //        }

    //        // Ejecutar ataque
    //        yield return StartCoroutine(attackPerformer.PerformAttack(attackWindowTime));
    //        yield return new WaitForSeconds(data.timeBetweenAttacks);
    //    }
    //}
    #endregion








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
