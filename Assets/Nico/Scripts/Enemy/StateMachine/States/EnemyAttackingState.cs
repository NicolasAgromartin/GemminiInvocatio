using System;
using UnityEngine;



public class EnemyAttackingState : EnemyBaseState
{
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override event Action<EnemyEvents> OnEventOccurred;


    // me faltaba esperar a que termine la animacion
    // antes de transicionar a el siguiente estado ( chase )

    private float elapsedTime;
    private readonly float attackCooldown = 3f;
    private bool canTransition;

    public override void EnterState()
    {
        elapsedTime = 0f;
        canTransition = false;
        agent.speed = 0f;

        animator.SetFloat("Movement", 0);
        animator.SetTrigger("Attack");

        animationEvents.OnAttackAnimationStarts += BlockTransitions;
        animationEvents.OnAttackAnimationEnd += EnableTransitions;
    }
    public override void ExitState()
    {
        animationEvents.OnAttackAnimationStarts -= BlockTransitions;
        animationEvents.OnAttackAnimationEnd -= EnableTransitions;

        animator.ResetTrigger("Attack");
    }
    public override void UpdateState()
    {
        if (targetsDetector.SelectedTarget == null)
        {
            if(canTransition) OnEventOccurred?.Invoke(EnemyEvents.TargetLost);
            else return;
        }
        else if (Vector3.Distance(transform.position, targetsDetector.SelectedTarget.transform.position) > agent.stoppingDistance)
        {
            if (canTransition) OnEventOccurred?.Invoke(EnemyEvents.TargetMoved);
            else return;
        }

        KeepLookingAt(targetsDetector.SelectedTarget);

        elapsedTime += Time.deltaTime;

        if(elapsedTime > attackCooldown)
        {
            animator.SetTrigger("Attack");
            elapsedTime = 0f;
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (!other.CompareTag("DamageDealer")) return;

        Weapon weapon = other.GetComponent<Weapon>();

        if (weapon.IsEnemy) return;
        else
        {
            if (canTransition)
            {
                OnEventOccurred?.Invoke(EnemyEvents.TakeHit);
            }
        }
    }
    


    private void KeepLookingAt(GameObject target)
    {
        if (target == null) return;

        Vector3 lookDirection;

        lookDirection = (target.transform.position - transform.position).normalized;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }


    private void BlockTransitions() => canTransition = false;
    private void EnableTransitions() => canTransition = true;
}
