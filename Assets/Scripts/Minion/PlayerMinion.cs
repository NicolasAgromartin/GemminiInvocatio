using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;




public class PlayerMinion : Fiend
{
    private GameObject player;
    private NavMeshAgent agent;
    [SerializeField] private TMP_Text currentAction;

    public float attackRange;
    private readonly float timeBetweenAttacks = 1f;
    private readonly float attackWindowTime = .5f;
    private readonly float maxRange = 20f;

    private bool suscribedToTacicts;

    private AttackPerformer attackPerformer;



    #region Life Cykle
    new private void Awake()
    {
        base.Awake();

        Destroy(GetComponent<Remains>());
        Destroy(GetComponent<SphereCollider>());

        attackPerformer = GetComponentInChildren<AttackPerformer>();
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
        TacticsSystem.OnQuickReturn -= ReturnToPlayer;
        TacticsSystem.OnSwarmOrder -= AttackTarget;

        TacticsSystem.OnPlayerMinionSelected -= SuscribeToTactics;
        TacticsSystem.OnMinionUnselected -= UnsuscribeToTactics;
    }
    #endregion



    public void SetMinionData(FiendSO data)
    {
        this.data = data;
        WeakenByResurrection();
    }
    private void WeakenByResurrection()
    {
        //Debug.Log(Stats.Health);
        Debug.Log(data.stats.Health);

        //Stats.Defense /= 2;
        //Stats.Attack /= 2;
        //Stats.Health /= 2;
    }




    #region Tactics System
    private void SuscribeToTactics(GameObject minionSelected)
    {

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

        if(suscribedToTacicts) UnsuscribeToTactics();
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
        while (enabled)
        {
            if (target.CompareTag("Enemy"))
            {
                // corrutina que cada cierto interavlo de tiempo
                // si el enemigo sigue en el rango de ataque lo ataca
                // si esta fuera del rango vuelvo a la corrutina de follow target
                agent.SetDestination(Vector3.Distance(transform.position, target.transform.position) > attackRange ? 
                    target.transform.position : transform.position);


                yield return StartCoroutine(PerformAttack());

            }
            else agent.SetDestination(target.transform.position);
            yield return null;
        }
    }
    private IEnumerator PerformAttack()
    {
        while (Vector3.Distance(agent.destination, transform.position) <= attackRange)
        {
            yield return StartCoroutine(attackPerformer.PerformAttack(attackWindowTime));
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }
    #endregion







    #region Wander Around
    ////private IEnumerator WanderAround()
    ////{
    ////    while (CheckPlayerDistance())
    ////    {
    ////        yield return new WaitForSeconds(timeBetweenWander);
    ////        agent.SetDestination(GenerateRandomPosition(player.transform.position));
    ////    }
    ////}

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
