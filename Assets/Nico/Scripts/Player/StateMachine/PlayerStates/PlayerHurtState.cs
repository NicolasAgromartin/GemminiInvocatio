using System;
using System.Collections;
using UnityEngine;

public class PlayerHurtState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;
    public PlayerHurtState(PlayerStateMachine stateMachine) : base(stateMachine) { }




    public override void EnterState()
    {
        attackPerformer.OnHurtEnded += HandleEndAnimation;
        animator.SetTrigger("DamageRecieved");
        //transform.GetComponent<Player>().StartCoroutine(RunHurtAnimation());
    }
    public override void ExitState() 
    {
        attackPerformer.OnHurtEnded -= HandleEndAnimation;
    }
    public override void UpdateState() { }





    private void HandleEndAnimation()
    {
        if(stats.CurrentHealth <= 0)
        {
            OnEventOccurred?.Invoke(TransitionEvent.Die);
        }
        else
        {
            animator.SetBool("Death", false);
            OnEventOccurred?.Invoke(TransitionEvent.End);
        }
    }

}
