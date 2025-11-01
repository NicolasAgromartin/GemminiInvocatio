using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;




public class PlayerMinion : Fiend
{
    private Weapon weapon;
    private GameObject player;
    private ControlCrown controlCrown;
    private readonly float timeReaction = 1f;
    private bool canMove = false;

    

    new private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Entity");

        Destroy(GetComponent<Remains>());
        Destroy(GetComponent<SphereCollider>());

        if (data != null) base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5f;
        
        weapon = GetComponentInChildren<Weapon>();
        weapon.GetComponent<SphereCollider>().enabled = false;
        weapon.SetIsEnemy(false);


        outlines.Clear();
        outlines.AddRange(GetComponentsInChildren<Outline>());
        if(outlines.Count > 0)
        {
            foreach(Outline outline in outlines)
            {
                outline.enabled = false;
            }
        }


        controlCrown = GetComponentInChildren<ControlCrown>(true);
        controlCrown.gameObject.SetActive(true);

        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<AnimationEvents>();
    }
    private void OnEnable()
    {
        animationEvents.OnResurrectAnimationEnd += EnableMovement;

        base.OnDamageRecieved += HandleDamage;
        base.OnDeath += HandleDeath;

        SceneLoader.OnSceneStartLoading += Reposition;
    }
    private void OnDisable()
    {
        animationEvents.OnResurrectAnimationEnd -= EnableMovement;
        
        base.OnDamageRecieved += HandleDamage;
        base.OnDeath -= HandleDeath;

        SceneLoader.OnSceneStartLoading -= Reposition;
    }
    private void Start()
    {
        player = FindAnyObjectByType<Player>().gameObject;
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Nico_Core"));
    }
    private void Update()
    {
        animator.SetFloat("Movement", Mathf.Clamp01(agent.velocity.magnitude / agent.speed));
    }





    private void Reposition()
    {
        Debug.Log("Repositioned");
        canMove = true;
        ReturnToPlayer();
        transform.position = player.transform.position;
    }

    private void HandleDamage(Unit minion, int currentHealth)
    {
        if (currentHealth > 0) animator.SetTrigger("Hurt");
    }
    private void HandleDeath(Unit minion)
    {
        animator.SetBool("IsDead", true);
        tag = "Untagged";
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<Rigidbody>());
        Debug.Log($"{minion} is dead");
        // cuando se termina la animacion se ejecuta DestroyGameObject();
        //Destroy(gameObject);
        StartCoroutine(Dissolve());
    }
    private IEnumerator Dissolve() 
    {
        yield return new WaitForSeconds(1f);

        Debug.Log(dissolver.Count);

        foreach(Dissolver d in dissolver)
        {
            yield return d.Dissolve(1);
        }
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
        //Stats.Attack /= 2;
        //Stats.MaxHealth /= 2;
    }

    // bloquear el movimiento hasta que haya revivido
    private void EnableMovement()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        canMove = true;
        agent.speed = chaseSpeed;
        GetComponentInChildren<Weapon>().SetWeaponDamage(Stats.Attack);
    }





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
        Debug.Log(target != null && Vector3.Distance(transform.position, target.transform.position) <= agent.stoppingDistance);

        while (target != null && Vector3.Distance(transform.position, target.transform.position) <= agent.stoppingDistance)
        {
            //Debug.Log(target);
            if (!target.CompareTag("Enemy")) yield break;
            Debug.Log($"{gameObject.name} is attacking {target.name}");

            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(3f);
        }
    }
}
