using System;
using System.Collections;
using UnityEngine;

public class PlayerHurtState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;
    public PlayerHurtState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        animator = stateMachine.Animator;
    }


    private readonly PlayerStateMachine stateMachine;
    private readonly Animator animator;

    public override void EnterState()
    {
        animator.SetTrigger("DamageRecieved");
        stateMachine.StartCoroutine(RunHurtAnimation());

        
    }
    public override void ExitState()
    {
        
    }
    public override void UpdateState()
    {
    }


    private IEnumerator RunHurtAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length / 2);
        OnEventOccurred?.Invoke(TransitionEvent.End);
    }





    #region Collisions
    public override void OnCollisionEnter(Collider other)
    {
        throw new NotImplementedException();
    }
    public override void OnCollisionExit(Collider other)
    {
        throw new NotImplementedException();
    }
    public override void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }
    public override void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }
    #endregion
}
