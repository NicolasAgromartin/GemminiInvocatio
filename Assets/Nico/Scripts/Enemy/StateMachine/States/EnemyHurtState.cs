using System;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    public EnemyHurtState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override event Action<EnemyEvents> OnEventOccurred;

    public override void EnterState()
    {
        animator.SetTrigger("Hurt");

        animationEvents.OnHurtAnimationEnd += HandleEndAnimation;
    }
    public override void ExitState()
    {
        animationEvents.OnHurtAnimationEnd -= HandleEndAnimation;
    }
    public override void UpdateState()
    {
    }


    private void HandleEndAnimation()
    {
        if (stats.CurrentHealth <= 0)
        {
            //animator.SetBool("IsDead", false);
            OnEventOccurred?.Invoke(EnemyEvents.Die);
        }
        else
        {
            OnEventOccurred?.Invoke(EnemyEvents.End);
        }
    }
}
