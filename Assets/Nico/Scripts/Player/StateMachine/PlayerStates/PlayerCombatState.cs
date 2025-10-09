using System;




public class PlayerCombatState : BaseState
{
    public override event Action<TransitionEvent> OnEventOccurred;

    public PlayerCombatState(PlayerStateMachine stateMachine) : base(stateMachine) { }




    #region Life Cykle
    public override void EnterState()
    {
        PerformAttack();
        attackPerformer.OnAttackEnded += EndAttack;
        attackPerformer.OnComboEnabled += EnableInput;
    }
    public override void ExitState()
    {
        attackPerformer.OnAttackEnded -= EndAttack;
        attackPerformer.OnComboEnabled -= EnableInput;
        InputManager.OnBasicAttackPerformed -= PerformAttack;
    }
    public override void UpdateState() 
    {
    }
    #endregion




    private void EnableInput()
    {
        InputManager.OnBasicAttackPerformed += PerformAttack;
    }



    private void PerformAttack()
    {
        animator.SetTrigger("Attack");
        InputManager.OnBasicAttackPerformed -= PerformAttack;
    }



    private void EndAttack()
    {
        OnEventOccurred?.Invoke(TransitionEvent.End);
    }



}
