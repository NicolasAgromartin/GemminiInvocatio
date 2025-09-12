using System;
using System.Collections;
using UnityEngine;




public class PlayerCombatState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerCombatState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        animator = stateMachine.Animator;
    }

    #region Components
    private readonly PlayerStateMachine stateMachine;
    private readonly Animator animator;
    #endregion


    private bool isAttacking;



    #region Life Cykle
    public override void EnterState()
    {
        isAttacking =false;
        
        InputManager.OnBasicAttackPerformed += BasicAttack;

        BasicAttack();
    }
    public override void ExitState()
    {
        InputManager.OnBasicAttackPerformed -= BasicAttack;
    }
    public override void UpdateState() 
    {
        if (!isAttacking)
        {
            OnEventOccurred?.Invoke(TransitionEvent.End);
        }
    }
    #endregion




    private void BasicAttack()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            stateMachine.StartCoroutine(SimulateAttack());
        }
    }
    private IEnumerator SimulateAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        isAttacking = false;
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
        
    }
    public override void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }
    #endregion
}
